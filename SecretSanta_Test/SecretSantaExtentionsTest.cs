using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SecretSanta.Extentions;

namespace SecretSanta_Test
{
    [TestClass]
    public class SecretSantaExtensionTest
    {
        private IList<Participant> _testList;

        [TestInitialize]
        public void SetUp()
        {
            _testList = new List<Participant>();
            _testList.Add(new Participant() { FirstName = "Name 1", LastName = "Last" });
            _testList.Add(new Participant() { FirstName = "Name 2", LastName = "Last" });
            _testList.Add(new Participant() { FirstName = "Name 3", LastName = "Last" });
            _testList.Add(new Participant() { FirstName = "Name 4", LastName = "Last" });
            _testList.Add(new Participant() { FirstName = "Name 5", LastName = "Last" });
        }

        [TestMethod]
        public void Helpers_GetShuffle_AllReturned_1000Tries()
        {
            for (int i = 0; i < 1000; i++)
            {
                var result = _testList.GetShuffle();

                foreach (var a in _testList)
                {
                    Assert.IsTrue(result.Contains(a));
                }
            }
        }

        [TestMethod]
        public void Helpers_GetPermutations_AllPermutationsReturned()
        {
            var result = _testList.GetPermutations().Count();
            var expected = Factoral(_testList.Count());

            Assert.AreEqual(expected, result, "There should be n! permutations, where n = {0}", _testList.Count());
        }

        private int Factoral(int n)
        {
            if (n <= 1)
                return 1;

            return n * Factoral(n - 1);
        }

        [TestMethod]
        public void Helpers_GetPermutations_AllUnique()
        {
            var result = _testList.GetPermutations().ToList();

            for (int current = 0; current < result.Count; current++)
            {
                for (int compare = current + 1; compare < result.Count; compare++)
                {
                    Assert.AreEqual(result[current].Count, result[compare].Count, "All lists should have the same number of elements");
                    CheckOrderingIsDifferent(result[current], result[compare]);
                }
            }
        }

        private void CheckOrderingIsDifferent<T>(IList<T> first, IList<T> second)
        {
            bool differenceDetected = false;
            for (int i = 0; i < first.Count; i++)
            {
                if (first[i].Equals(second[i]))
                {
                    differenceDetected = true;
                    break;
                }
            }

            Assert.IsTrue(differenceDetected, "No difference was found");
        }

        [TestMethod]
        public void Helpers_ToDictionary_ReturnsDictionary()
        {
            var pairs = GetEnumKVPairs().ToList();
            var result = pairs.ToDictionary();

            Assert.AreEqual(pairs.Count(), result.Count);

            foreach (var pair in pairs)
            {
                Assert.IsTrue(result.Contains(pair));
            }
        }

        private IEnumerable<KeyValuePair<Participant, Participant>> GetEnumKVPairs()
        {
            for (int i = 0; i < _testList.Count; i++)
            {
                if (i < _testList.Count - 1)
                {
                    yield return new KeyValuePair<Participant, Participant>(_testList[i], _testList[i + 1]);
                }
                else
                {
                    yield return new KeyValuePair<Participant, Participant>(_testList[i], _testList[0]);
                }
            }
        }

        [TestMethod]
        public void Helpers_ZipToKV_ReturnsValidZip()
        {
            var numberList = new List<int>() { 1, 2, 3, 4, 5 };
            var result = numberList.ZipToKV(numberList).ToList();

            Assert.AreEqual(numberList.Count, result.Count(), "Zipped list should eb same length");

            foreach (var pair in result)
            {
                Assert.AreEqual(pair.Key, pair.Value, "Values did not match");
            }
        }
    }
}
