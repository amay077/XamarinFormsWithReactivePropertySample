
using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections;
using Codeplex.Reactive;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System.Linq;
using System.Diagnostics;
using System.Reactive.Concurrency;

namespace RxPropertyTest
{
    [TestFixture]
    public class ReactivePropertyValidationTest
    {
        private TestTarget target;

        [SetUp]
        public void Initialize()
        {
            this.target = new TestTarget();
        }

        [TearDown]
        public void Cleanup()
        {
            this.target = null;
        }

        [Test]
        public void InitialState()
        {
            Assert.IsTrue(target.RequiredProperty.HasErrors);
        }

        [Test]
        public void AnnotationTest()
        {
            var errors = new List<IEnumerable>();
            target.RequiredProperty
                .ObserveErrorChanged
                .Where(x => x != null)
                .Subscribe(errors.Add);
            Assert.AreEqual(0, errors.Count);

            target.RequiredProperty.Value = "a";
            Assert.AreEqual(0, errors.Count);
            Assert.IsFalse(target.RequiredProperty.HasErrors);

            target.RequiredProperty.Value = null;
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("error!", errors[0].Cast<string>().First());
            Assert.IsTrue(target.RequiredProperty.HasErrors);
        }

        [Test]
        public void BothTest()
        {
            IEnumerable error = null;
            target.BothProperty
                .ObserveErrorChanged
                .Subscribe(x => error = x);

            Assert.IsTrue(target.BothProperty.HasErrors);
            Assert.IsNull(error);

            target.BothProperty.Value = "a";
            Assert.IsFalse(target.BothProperty.HasErrors);
            Assert.IsNull(error);

            target.BothProperty.Value = "aaaaaa";
            Assert.IsTrue(target.BothProperty.HasErrors);
            Assert.IsNotNull(error);
            Assert.AreEqual("5over", error.Cast<string>().First());

            target.BothProperty.Value = null;
            Assert.IsTrue(target.BothProperty.HasErrors);
            Assert.AreEqual("required", error.Cast<string>().First());
        }

        [Test]
        public void TaskTest()
        {
            var errors = new List<IEnumerable>();
            target.TaskValidationTestProperty
                .ObserveErrorChanged
                .Where(x => x != null)
                .Subscribe(errors.Add);
            Assert.AreEqual(0, errors.Count);

            target.TaskValidationTestProperty.Value = "a";
            Assert.IsFalse(target.TaskValidationTestProperty.HasErrors);
            Assert.AreEqual(0, errors.Count);

            target.TaskValidationTestProperty.Value = null;
            Assert.IsTrue(target.TaskValidationTestProperty.HasErrors);
            Assert.AreEqual(1, errors.Count);
        }

        [Test]
        public void AsyncValidation_SuccessCase()
        {
            var tcs     = new TaskCompletionSource<string>();
            var rprop   = new ReactiveProperty<string>().SetValidateNotifyError(_ => tcs.Task);
            IEnumerable error = null;
            rprop.ObserveErrorChanged.Subscribe(x => error = x);
            Assert.IsFalse(rprop.HasErrors);
            Assert.IsNull(error);

            var task = Task.Factory.StartNew(() => 
            {
                rprop.Value = "dummy";  //--- push value
                tcs.SetResult(null);    //--- validation success!
            });

            task.Wait();

            Assert.IsFalse(rprop.HasErrors);
            Assert.IsNull(error);
        }

        [Test]
        public void AsyncValidation_FailedCase()
        {
            var tcs     = new TaskCompletionSource<string>();
            var rprop   = new ReactiveProperty<string>().SetValidateNotifyError(_ => tcs.Task);

            IEnumerable error = null;
            rprop.ObserveErrorChanged.Subscribe(x => error = x);

            Assert.IsFalse(rprop.HasErrors);
            Assert.IsNull(error);

            var errorMessage    = "error occured!!";
            var task = Task.Factory.StartNew(() => 
            {
                rprop.Value = "dummy";  //--- push value
                tcs.SetResult(errorMessage);    //--- validation error!
            });

            task.Wait();

            Assert.IsTrue(rprop.HasErrors);
            Assert.IsNotNull(error);
            Assert.AreEqual(errorMessage, error.Cast<string>().First());
            Assert.AreEqual(errorMessage, rprop.GetErrors("Value").Cast<string>().First());
        }

//        [Test]
//        public void AsyncValidation_ThrottleTest()
//        {
//            var scheduler = DefaultScheduler.Instance;
//            var rprop       = new ReactiveProperty<string>()
//                .SetValidateNotifyError(xs =>
//                {
//                    return  xs
//                        .Throttle(TimeSpan.FromSeconds(1), scheduler)
//                        .Select(x => string.IsNullOrEmpty(x) ? "required" : null);
//                });
//
//            IEnumerable error = null;
//            rprop.ObserveErrorChanged.Subscribe(x => error = x);
//
//            scheduler.AdvanceTo(TimeSpan.FromMilliseconds(0).Ticks);
//            rprop.Value = string.Empty;
//            Assert.IsFalse(rprop.HasErrors);
//            Assert.IsNull(error);
//
//            scheduler.AdvanceTo(TimeSpan.FromMilliseconds(300).Ticks);
//            rprop.Value = "a";
//            Assert.IsFalse(rprop.HasErrors);
//            Assert.IsNull(error);
//
//            scheduler.AdvanceTo(TimeSpan.FromMilliseconds(700).Ticks);
//            rprop.Value = "b";
//            Assert.IsFalse(rprop.HasErrors);
//            Assert.IsNull(error);
//
//            scheduler.AdvanceTo(TimeSpan.FromMilliseconds(1100).Ticks);
//            rprop.Value = string.Empty;
//            Assert.IsFalse(rprop.HasErrors);
//            Assert.IsNull(error);
//
//            scheduler.AdvanceTo(TimeSpan.FromMilliseconds(2500).Ticks);
//            Assert.IsTrue(rprop.HasErrors);
//            Assert.IsNotNull(error);
//            Assert.AreEqual("required", error.Cast<string>().First());
//        }
    }

    class TestTarget
    {
        [Required(ErrorMessage = "error!")]
        public ReactiveProperty<string> RequiredProperty { get; private set; }

        [StringLength(5, ErrorMessage = "5over")]
        public ReactiveProperty<string> BothProperty { get; private set; }

        public ReactiveProperty<string> TaskValidationTestProperty { get; private set; }

        public TestTarget()
        {
            this.RequiredProperty = new ReactiveProperty<string>()
                .SetValidateAttribute(() => RequiredProperty);

            this.BothProperty = new ReactiveProperty<string>()
                .SetValidateAttribute(() => BothProperty)
                .SetValidateNotifyError(s => string.IsNullOrWhiteSpace(s) ? "required" : null);

            this.TaskValidationTestProperty = new ReactiveProperty<string>()
                .SetValidateNotifyError(async s =>
                {
                    if (string.IsNullOrWhiteSpace(s))
                    {
                        return await Task.FromResult("required");
                    }
                    return await Task.FromResult((string) null);
                });
        }
    }
}
