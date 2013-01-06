using System;
using NUnit.Framework;
using NUnit.Framework;
using Target = Kean.Core.Collection;

namespace Kean.Core.Collection.Test
{
    [TestFixture]
    public class ReadOnly :
        Kean.Test.Fixture<ReadOnly>
    {
        string prefix = "Kean.Core.Collection.Test.";
        protected override void Run()
        {
            this.Run(this.Create, this.Equality, this.Count);
        }
        [Test]
        public void Create()
        {
            Target.ReadOnlyVector<int> vector = new ReadOnlyVector<int>(0, 1, 2, 3, 4);
            Expect(vector[0], Is.EqualTo(0), this.prefix + "Create.0");
            Expect(vector[1], Is.EqualTo(1), this.prefix + "Create.1");
            Expect(vector[2], Is.EqualTo(2), this.prefix + "Create.2");
            Expect(vector[3], Is.EqualTo(3), this.prefix + "Create.3");
            Expect(vector[4], Is.EqualTo(4), this.prefix + "Create.4");
        }
        [Test]
        public void Equality()
        {
            Target.ReadOnlyVector<int> vector1 = new ReadOnlyVector<int>(0, 1, 2, 3, 4);
            Target.ReadOnlyVector<int> vector2 = new ReadOnlyVector<int>(0, 1, 2, 3, 4);
            Target.ReadOnlyVector<int> vector3 = new ReadOnlyVector<int>(0, 1, 20, 3, 4);
            Expect(vector1, Is.EqualTo(vector2), this.prefix + "Equality.0");
            Expect(vector1, Is.Not.EqualTo(vector3), this.prefix + "Equality.1");
        }
        [Test]
        public void Count()
        {
            Target.ReadOnlyVector<int> vector = new ReadOnlyVector<int>();
            Expect(vector.Count, Is.EqualTo(0), this.prefix + "Count.0");
            vector = new ReadOnlyVector<int>(1,2,3);
            Expect(vector.Count, Is.EqualTo(3), this.prefix + "Count.1");
        }
    }
}
