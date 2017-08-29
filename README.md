# Vdf.NET JsonConverter

[![NuGet](https://img.shields.io/nuget/v/Gameloop.Vdf.JsonConverter.svg?style=flat-square)](https://www.nuget.org/packages/Gameloop.Vdf.JsonConverter)
[![AppVeyor](https://img.shields.io/appveyor/ci/Shravan2x/gameloop-vdf-jsonconverter.svg?maxAge=2592000&style=flat-square)](https://ci.appveyor.com/project/Shravan2x/gameloop-vdf-jsonconverter)

VDF-JSON converters for [Vdf.NET](https://github.com/shravan2x/Gameloop.Vdf).

## Documentation

This extension should be used _after_ deserializing data.

To convert some `VObject` _importantVdfObject_ to a corresponding **Json.NET** `JObject` _importantJsonObject_, do
```c#
JObject importantJsonObject = importantVdfObject.ToJson();
```

Correspondingly, to convert some file _importantInfo.vdf_ to a JSON _importantInfo.json_, do
```c#
dynamic volvo = VdfConvert.Deserialize(File.ReadAllText("importantInfo.vdf"));
File.WriteAllText("importantInfo.json", volvo.ToJson());
```
The `.ToJson()` and `.ToVdf()` extension methods are defined on all `VToken` and `JToken` instances respectively.

## FAQ

#### Why doesn't calling `someJsonObject.ToVdf().ToJson()` always return the original Json?

This is because of the way some JSON elements are converted to VDF such as arrays and nulls.

JSON
```json
{
  "image_url": null,
  "used_by_classes": [
    "Scout",
    "Engineer"
  ]
}
```
VDF
```
{
  "image_url" ""
  "used_by_classes"
  {
    "0" "Scout"
    "1" "Engineer"
  }
}
```

As such, when these elements are converted back to JSON, they lose their original structure and become:

JSON
```json
{
  "image_url": "",
  "used_by_classes": {
    "0": "Scout",
    "1": "Engineer"
  }
}
```

## License

Vdf.NET.JsonConverter is released under the [MIT license](https://opensource.org/licenses/MIT).
