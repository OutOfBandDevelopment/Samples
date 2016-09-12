# Collection of Useful Classes

These classes are provided under the MIT license from Out-Of-Band Development. (c) 2016

## SmtpClientService

SmtpClientService is an implementation of the Microsoft.AspNet.Identity.IIdentityMessageService 
interface that uses System.Net.Mail.SmtpClient.

## Code39

Simple class to generate Code39 barcodes using GDI+.

## ConsoleEx

ConsoleEX allows you to create simple interactive user prompts in command line 
tool that allow default values and obfuscated data entry for items such as passwords.

## XFragment

XFragment allows for XML Fragments to to used with LINQ to XML. This is very useful with 
EntityFramework and XML fields in Microsoft SQL Server.  (Now Mutable!)

## CsvWriter

CsvWriter allows any enumerable set of object to be serialized into a comma-seperate values.
This will even work with IQueriable<> from Entity Framework.  All fields and columns are quoted
and escaped based on [RFC 4180](https://tools.ietf.org/html/rfc4180).  Underscores in property
names will be converted to spaces for field labels.