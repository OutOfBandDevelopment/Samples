# Collection of Useful Classes

These classes are provided under the MIT license from Out-Of-Band Development. (c) 2016

## [SmtpClientService](SmtpClientService.cs)

SmtpClientService is an implementation of the Microsoft.AspNet.Identity.IIdentityMessageService 
interface that uses System.Net.Mail.SmtpClient.

## [Code39](Code39.cs)

Simple class to generate Code39 barcodes using GDI+.

## [ConsoleEx](ConsoleEx.cs)

ConsoleEX allows you to create simple interactive user prompts in command line 
tool that allow default values and obfuscated data entry for items such as passwords.

## [XFragment](XFragment.cs)

XFragment allows for XML Fragments to to used with LINQ to XML. This is very useful with 
EntityFramework and XML fields in Microsoft SQL Server.  (Now Mutable!)

## [CsvWriter](CsvWriter.cs)

CsvWriter allows any enumerable set of object to be serialized into a comma-seperate values.
This will even work with IQueriable<> from Entity Framework.  All fields and columns are quoted
and escaped based on [RFC 4180](https://tools.ietf.org/html/rfc4180).  Underscores in property
names will be converted to spaces for field labels.

## [IniFile](IniFile.cs)

IniFile is a wrapper around the Win32 operations for reading and writing from INI files. 

## [Convert](Convert.vb)

Convert is a set of method to encode and decode binary data as [Base 64](https://en.wikipedia.org/wiki/Base64),
[Base 32](https://en.wikipedia.org/wiki/Base32), [Base 16](https://en.wikipedia.org/wiki/Hexadecimal) and 
[Base 8](https://en.wikipedia.org/wiki/Octal)
