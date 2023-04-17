namespace Lib.Contracts.Generics;

public class ItemsResponse<TResponseType>
{
  public IEnumerable<TResponseType>? Items { get; set; }
}