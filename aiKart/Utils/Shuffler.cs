namespace aiKart.Utils;
public class Shuffler<T> where T : class
{
  private Random random = new Random();

  public List<T> Shuffle<A>(A items)
    where A : IEnumerable<T>
  {
    List<T> itemList = items.ToList();
    int n = itemList.Count;


    for (int i = n - 1; i > 0; i--)
    {

      // Pick a random index
      // from 0 to i
      int j = random.Next(0, i + 1);

      // Swap arr[i] with the
      // element at random index
      T temp = itemList[i];
      itemList[i] = itemList[j];
      itemList[j] = temp;
    }

    return itemList;
  }

}