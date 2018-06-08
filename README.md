# nebulas .NET sdk 星云链.NET SDK
本SDK基于.NET Standard 2.0开发(除Test项目为.NET Framework 4.6.1)。
可以被.NET CORE、.NET Framework、Xamarin等项目调用。

本SDK在遵守C#开发相关规范的前提下，
尽量与官方JS SDK的类结构、属性名、方法名保持一致，让使用者以最小学习成本使用。

使用示例：
```C#
  string host = "https://mainnet.nebulas.io";
  string from = "n1TA6on2ikjjUcpwbtjjcsAgHTP7fEZ41Bk";
  string to = "n1o5DKJefXgFNLhUTXRiJFFrTp2npgSuhvW";
  string value = "0";
  string nonce = "0";
  int gasPrice = 1000000;
  int gasLimit = 2000000;
  string function = "getIntegralByPage";
  string args = "[]";
  Nebulas.Neb neb = new Neb(new HttpRequest(host));
  string result = neb.API.CallAsync(from, to, value, nonce, gasPrice, gasLimit, function, args).Result;
```


当前是回复官方邮件的最小可行性验证版本。
