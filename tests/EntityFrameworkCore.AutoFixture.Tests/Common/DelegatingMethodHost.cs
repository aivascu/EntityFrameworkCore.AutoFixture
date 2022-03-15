using System;

namespace EntityFrameworkCore.AutoFixture.Tests.Core
{
    public class DelegatingMethodHost<T>
    {
        public Func<T> OnMethodCall { get; set; } = () => default;

        public virtual T DoStuff() => this.OnMethodCall.Invoke();
    }
}
