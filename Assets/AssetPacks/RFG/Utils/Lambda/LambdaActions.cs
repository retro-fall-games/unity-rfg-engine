using System;
using System.Collections.Generic;

namespace RFG
{
  public static class LambdaActions<T>
  {
    public static Dictionary<string, Action<T>> Actions = new Dictionary<string, Action<T>>();
  }
}