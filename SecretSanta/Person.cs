using System;

namespace SecretSanta
{
    public class Person : IComparable, IComparable<Person>, IEquatable<Person>
    {
        public Person(string name, string email)
        {
            Name = name;
            Email = email;
        }

        public string Name { get; }
        public string Email { get; }

        public int CompareTo(object obj)
        {
            if (obj is Person other)
                return CompareTo(other);

            return -1;
        }

        public int CompareTo(Person other)
        {
            return string.Compare(Name, other.Name, StringComparison.Ordinal);
        }

        public bool Equals(Person other)
        {
            if (other == null)
                return false;

            return Name.Equals(other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (ReferenceEquals(obj, null))
            {
                return false;
            }

            if (obj is Person other)
                return Equals(other);

            return false;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}