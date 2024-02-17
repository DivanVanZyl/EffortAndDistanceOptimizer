using System.Runtime.CompilerServices;

namespace Optimizations
{
    public class SpaciousInsert
    {
        public int NextOptimalIndex<T>(IReadOnlyList<T> collection, Func<T, bool>? isEmptyElementFunc = null, double effortWeight = 1, double closenessWeight = 7)
        {
            int optimalIndex = 0;
            int closestOccupiedIndexToLeft = -1;
            int closestOccupiedIndexToRight = -1;
            double leastBadValue = double.MaxValue;
            bool collectionFull = true; //We cannot return a meaningful value as the optimal index, if the collection is already full.
            bool possibleElementsToTheRight = true; //If there are no elements to the right, we do not run the code to check forward again.

            for (int currentIndex = 0; currentIndex < collection.Count; currentIndex++)
            {
                bool isEmpty = false;
                if (isEmptyElementFunc is not null)
                {
                    isEmpty = isEmptyElementFunc(collection[currentIndex]);
                }
                else
                {
                    isEmpty = collection[currentIndex].isEmptyElement();
                }

                //Only empty elements are considered for returning as the optimal index.
                if (isEmpty)
                {
                    collectionFull = false;
                    double closenessValue = 0.0;
                    //Determine number of elements between current index and the occupied index to the left.
                    if (closestOccupiedIndexToLeft != -1)
                    {
                        closenessValue += ClosenessValue(currentIndex - (closestOccupiedIndexToLeft + 1), closenessWeight);
                    }

                    //Determine number of elements between current index and the occupied index to the right.
                    if (possibleElementsToTheRight)
                    {
                        if (currentIndex != collection.Count - 1)    //If we are on the last element in the collection, we don't have to check for an element to the right of the current element.
                        {
                            if (closestOccupiedIndexToRight == -1)
                            {
                                for (int cntForward = currentIndex; cntForward < collection.Count; cntForward++)
                                {
                                    if (!collection[cntForward].isEmptyElement())
                                    {
                                        closestOccupiedIndexToRight = cntForward;
                                        closenessValue += ClosenessValue(closestOccupiedIndexToRight - (currentIndex + 1), closenessWeight);
                                        break;
                                    }
                                }
                                if (closestOccupiedIndexToRight == -1)
                                    possibleElementsToTheRight = false; //There are no occupied elements to the right anymore, because we have just completed the search to the right and found no occupied elements.
                            }
                            else
                            {
                                closenessValue += ClosenessValue(closestOccupiedIndexToRight - (currentIndex + 1), closenessWeight);
                            }
                        }
                    }

                    //Determine badness
                    var thisElementsValue = EffortValue(currentIndex, effortWeight) + closenessValue;
                    if (thisElementsValue < leastBadValue)
                    {
                        leastBadValue = thisElementsValue;
                        optimalIndex = currentIndex;
                    }
                }
                else
                {
                    closestOccupiedIndexToLeft = currentIndex;  //Adjust left occupied space
                    closestOccupiedIndexToRight = -1;   //Adjust right occupied space. We reset this here. On the next iteration, it will search ahead for the next occupied element.
                }
            }

            //Return final result
            if (collectionFull)
            {
                throw new InvalidOperationException("Collection is full. Cannot return the optimal insertion index.");
            }
            return optimalIndex;
        }

        private double EffortValue(int index, double weight)
        {
            return index * weight;
        }

        private double ClosenessValue(int spacesBetweenIndexes, double weight)
        {
            return weight * (Math.Pow(Math.E, -2 * spacesBetweenIndexes));
        }
    }


    internal static class CollectionExtensions
    {
        internal static bool isEmptyElement<T>(this T element)
        {
            if (element is int || element is long || element is float || element is short || element is double)
            {
                return Convert.ToDouble(element) == 0.0;
            }
            if (element is bool)
            {
                return !Convert.ToBoolean(element);
            }
            if (element is string)
            {
                if (string.IsNullOrEmpty(Convert.ToString(element)))
                    return true;
                return false;
            }

            //If you reach this code, the element's type could not be determined.
            string message;
            if (element is null)
            {
                message = "Could not evaluate type for null element;";
            }
            else
            {
                message = "Could not evaluate type: " + element.GetType() + " please pass a delegate that defines the check for occupation.";
            }
            throw new NotSupportedException(message);
        }
    }
}