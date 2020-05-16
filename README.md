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

#### Handling duplicate keys when converting to JSON

Unlike VDF, the JSON format doesn't support duplicate keys in its objects. So this library allows adjusting duplicate key handling via the `ObjectDuplicateKeyHandling` and `ValueDuplicateKeyHandling` settings. They can be set when calling `.ToJson()` like
```c#
.ToJson(new VdfJsonConversionSettings {
  // Sets duplicate key handling when the corresponding value is an object or list
  ObjectDuplicateKeyHandling = DuplicateKeyHandling.Ignore,
  // Sets duplicate key handling when the corresponding value is a singular value
  ValueDuplicateKeyHandling = DuplicateKeyHandling.Ignore
})
```

The different duplicate key handling options are:
- `Ignore` - Ignores all duplicate keys after the first. Selecting this option will cause all duplicates to be discarded.
- `Merge` - Merges two `JObject`s or `JArray`s using the [`.Merge()` method provided by Json.NET](https://www.newtonsoft.com/json/help/html/MergeJson.htm) with default settings.
- `Replace` - Ignores all duplicate keys before the last. Selecting this option will cause all duplicates to be discarded.
- `Throw` - Simply throws an exception when a duplicate key in encountered. This is the default behavior.

## FAQ

#### Why doesn't calling `someJsonObject.ToVdf().ToJson()` always return the original JSON?

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
