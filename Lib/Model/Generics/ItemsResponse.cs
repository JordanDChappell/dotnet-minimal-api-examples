namespace Lib.Model.Generics;

public class ItemsResponse<TResponseType>
{
  public IEnumerable<TResponseType>? Items { get; set; }
}