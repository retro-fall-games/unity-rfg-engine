using System;
using System.Collections.Generic;

namespace RFG.Utils
{
  public static class LambdaActions<T>
  {
    public static Dictionary<string, Action<T>> Actions = new Dictionary<string, Action<T>>();
  }
}