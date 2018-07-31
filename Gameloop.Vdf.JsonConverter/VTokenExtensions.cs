﻿using Gameloop.Vdf.Linq;
using Newtonsoft.Json.Linq;
using System;

namespace Gameloop.Vdf.JsonConverter
{
    public static class VTokenExtensions
    {
        public static JToken ToJson(this VToken tok, VdfJsonConversionSettings settings = null)
        {
            if (settings == null)
                settings = new VdfJsonConversionSettings();

            switch (tok)
            {
                case VValue val:
                    return val.ToJson();
                case VProperty prop:
                    return prop.ToJson(settings);
                case VObject obj:
                    return obj.ToJson(settings);
                default:
                    throw new InvalidOperationException("Unrecognized VToken.");
            }
        }

        public static JValue ToJson(this VValue val)
        {
            return new JValue(val.Value);
        }

        public static JProperty ToJson(this VProperty prop, VdfJsonConversionSettings settings = null)
        {
            if (settings == null)
                settings = new VdfJsonConversionSettings();

            return new JProperty(prop.Key, prop.Value.ToJson(settings));
        }

        public static JObject ToJson(this VObject obj, VdfJsonConversionSettings settings = null)
        {
            if (settings == null)
                settings = new VdfJsonConversionSettings();

            JObject resultObj = new JObject();

            foreach (VProperty prop in obj.Children())
            {
                if (!resultObj.ContainsKey(prop.Key))
                    resultObj.Add(prop.ToJson(settings));
                else if (resultObj[prop.Key] is JValue)
                    HandleValueDuplicateKey(resultObj, prop, settings);
                else
                    HandleObjectDuplicateKey(resultObj, prop, settings);
            }

            return resultObj;
        }

        private static void HandleValueDuplicateKey(JObject baseObj, VProperty prop, VdfJsonConversionSettings settings)
        {
            switch (settings.ValueDuplicateKeyHandling)
            {
                case DuplicateKeyHandling.Ignore:
                    break;

                case DuplicateKeyHandling.Replace:
                    baseObj[prop.Key] = prop.Value.ToJson(settings);
                    break;

                case DuplicateKeyHandling.Throw:
                    throw new Exception($"Key '{prop.Key}' already exists in object.");
            }
        }

        private static void HandleObjectDuplicateKey(JObject baseObj, VProperty prop, VdfJsonConversionSettings settings)
        {
            switch (settings.ObjectDuplicateKeyHandling)
            {
                case DuplicateKeyHandling.Ignore:
                    break;

                case DuplicateKeyHandling.Merge:
                    JObject prevObj = baseObj[prop.Key] as JObject;
                    if (prevObj == null)
                        throw new Exception("Unable to merge since values are of different types.");
                    prevObj.Merge(prop.Value.ToJson(settings));
                    break;

                case DuplicateKeyHandling.Replace:
                    baseObj[prop.Key] = prop.Value.ToJson(settings);
                    break;

                case DuplicateKeyHandling.Throw:
                    throw new Exception($"Key '{prop.Key}' already exists in object.");
            }
        }
    }

    public class VdfJsonConversionSettings
    {
        private DuplicateKeyHandling _valueDuplicateKeyHandling = DuplicateKeyHandling.Throw;

        public DuplicateKeyHandling ObjectDuplicateKeyHandling { get; set; } = DuplicateKeyHandling.Throw;

        public DuplicateKeyHandling ValueDuplicateKeyHandling
        {
            get => _valueDuplicateKeyHandling;

            set
            {
                if (value == DuplicateKeyHandling.Merge)
                    throw new Exception("This duplicate key handling option in invalid for VDF values.");

                _valueDuplicateKeyHandling = value;
            }
        }
    }

    public enum DuplicateKeyHandling
    {
        Ignore,
        Merge,
        Replace,
        Throw
    }
}
