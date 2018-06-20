# nebulas .NET sdk 星云链.NET SDK
本SDK基于.NET Standard 2.0开发(除Test项目为.NET Framework 4.6.1)。<br>
可以被.NET CORE、.NET Framework、Xamarin等项目调用。<br>

本SDK在遵守C#开发相关规范的前提下，<br>
尽量与官方JS SDK的类结构、属性名、方法名保持一致，让使用者以最小学习成本使用。<br>

文档请参照官方[JS SDK](https://nebulasio.github.io/neb.js/index.html)文档，即可。

使用示例：

Call方法 
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
  dynamic result = neb.API.CallAsync(from, to, value, nonce, gasPrice, gasLimit, function, args).Result;
```

GetAccountState 
```C#
    HttpRequest request = new HttpRequest(_host);
    API api = new API(request);
    var accountState = api.GetAccountStateAsync("n1TA6on2ikjjUcpwbtjjcsAgHTP7fEZ41Bk").Result;
    int nonce = ++accountState.Result.Nonce;
```

SendRawTransaction 
```C#
    string function = "addIntegral";
    string args = "[\"哈哈哈测试一下\",\"40000\"]";
    var fromAccount = new Account("ab14bca2fd7703b76972a696a6df4ebeb45f20d01086d695b46b6120adbae4d9");
    var tx = new Transaction(1001, fromAccount, to, ulong.Parse(value), ulong.Parse(nonce.ToString()), (ulong)gasPrice, (ulong)gasLimit, function, args);
    tx.SignTransaction();

    var result = api.SendRawTransactionAsync(tx.toProtoString()).Result;
```

GetTransactionReceipt
```C#
    Nebulas.Neb neb = new Neb(new HttpRequest(_host));
    var result = neb.API.GetTransactionReceiptAsync("ef2308cb962188481f879c159117fbd08bb7e3e1cf56f0163ab21c7ec31930cc").Result;
```
