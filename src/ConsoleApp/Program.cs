﻿// ReSharper disable LocalizableElement

namespace ConsoleApp
{
  using System;
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.Diagnostics.CodeAnalysis;
  using System.IO;
  using SimpleExcelExporter;
  using SimpleExcelExporter.Definitions;

  [SuppressMessage("Security", "CA5394:Do not use insecure randomness", Justification = "Weak pseudo-random numbers aren't used in a security-sensitive manner")]
  public static class Program
  {
    public static void Main()
    {
      // First test : try to create empty Excel file
      var n = DateTime.Now;
      var tempDi = new DirectoryInfo($"ExampleOutput-{n.Year - 2000:00}-{n.Month:00}-{n.Day:00}-{n.Hour:00}{n.Minute:00}{n.Second:00}");
      tempDi.Create();

      GenerateSpreadSheetFromWorkbookDfn(tempDi);
      GenerateSpreadSheetFromAnnotatedDataEmpty(tempDi);
      GenerateSpreadSheetFromAnnotatedData(tempDi);
      GenerateBigSpreadsheetFromAnnotatedData(tempDi);
      GenerateBigSpreadsheetFromWorkBookDfn(tempDi);
      GenerateSpreadsheetFromGroup(tempDi);
    }

    private static void GenerateSpreadsheetFromGroup(DirectoryInfo tempDi)
    {
      Console.WriteLine("GenerateBigSpreadsheetFromAnnotatedData");
      using var memoryStream = new MemoryStream();
      using var streamWriter = new StreamWriter(memoryStream);
      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Start();
      var group = new Group();
      var person = new Person { Name = "0" };
      group.Persons.Add(person);
      Person currentPerson = person;
      for (int i = 1; i < 2; i++)
      {
        var child = new Person { Name = $"{i}_0" };
        currentPerson.Children.Add(child);

        for (int j = 1; j < 16383; j++)
        {
          child = new Person { Name = $"{i}_{j}" };
          currentPerson.Children.Add(child);
        }

        currentPerson = child;
      }

      stopwatch.Stop();
      Console.WriteLine($"Done in {stopwatch.Elapsed.Seconds} seconds !");

      Console.WriteLine("Instantiating the SpreadsheetWriter...");
      stopwatch.Reset();
      stopwatch.Start();
      SpreadsheetWriter spreadsheetWriter = new SpreadsheetWriter(streamWriter.BaseStream, group);
      stopwatch.Stop();
      Console.WriteLine($"Done in {stopwatch.Elapsed.Seconds} seconds !");

      Console.WriteLine("Writing the Excel file...");
      stopwatch.Reset();
      stopwatch.Start();
      spreadsheetWriter.Write();
      stopwatch.Stop();
      Console.WriteLine($"Done in {stopwatch.Elapsed.Seconds} seconds !");

      using FileStream file = new FileStream(Path.Combine(tempDi.FullName, "TestWithData5.xlsx"), FileMode.Create, FileAccess.Write);
      memoryStream.WriteTo(file);
    }

    private static void GenerateBigSpreadsheetFromAnnotatedData(DirectoryInfo tempDi)
    {
      Console.WriteLine("GenerateBigSpreadsheetFromAnnotatedData");
      using var memoryStream = new MemoryStream();
      using var streamWriter = new StreamWriter(memoryStream);
      var team = new Team();
      Random rnd = new Random();
      WeakPseudoRandomDateTime weakPseudoRandomDate = new WeakPseudoRandomDateTime();
      Console.WriteLine("Generating the players...");
      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Start();

      var nullPlayer = new Player
      {
        HeaderName0 = "klqsfjslfj",
        HeaderName1 = "qslkfjlm",
        PlayerCode = "Code0",
        PlayerName = null,
        PracticeTime = null,
        Size = null,
        DateOfBirth = null,
        GamePlayed = 0,
        IsActiveFlag = null,
        NumberOfVictory = null,
        FieldGoalPercentage = null,
        Salary = null,
        MaleChildren = null,
        FemaleChildren = new List<Child>
           {
             new () { FirstName = "FirstName1", Age = 1 },
             new () { FirstName = "FirstName2", Age = 2 },
           },
      };
      team.Players.Add(nullPlayer);

      for (int i = 1; i < 1000000; i++)
      {
        var player = new Player
        {
          PlayerCode = $"Code{i}",
          PlayerName = $"Player{i}",
          PracticeTime = new TimeSpan(rnd.Next(0, 23), rnd.Next(0, 59), 0),
          Size = rnd.Next(18, 100),
          DateOfBirth = weakPseudoRandomDate.Next(),
          IsActiveFlag = Convert.ToBoolean(rnd.Next(0, 100) % 2),
          NumberOfVictory = rnd.Next(0, 100),
          FieldGoalPercentage = Convert.ToDouble(rnd.Next(0, 100)) / 100,
          Salary = Convert.ToDecimal(rnd.Next(2000, 1000000) + 0.12654984m),
          GamePlayed = i,
          MaleChildren = new List<Child>
          {
            new () { FirstName = "NephewFirstName1", Age = 1 },
          },
          FemaleChildren = new List<Child>
           {
             new () { FirstName = "FirstName1", Age = 1 },
           },
        };

        if (i % 2 == 0)
        {
          player.FemaleChildren.Add(new Child { FirstName = "FirstName2", Age = 2 });
        }

        team.Players.Add(player);
      }

      stopwatch.Stop();
      Console.WriteLine($"Done in {stopwatch.Elapsed.Seconds} seconds !");

      Console.WriteLine("Instantiating the SpreadsheetWriter...");
      stopwatch.Reset();
      stopwatch.Start();
      SpreadsheetWriter spreadsheetWriter = new SpreadsheetWriter(streamWriter.BaseStream, team);
      stopwatch.Stop();
      Console.WriteLine($"Done in {stopwatch.Elapsed.Seconds} seconds !");

      Console.WriteLine("Writing the Excel file...");
      stopwatch.Reset();
      stopwatch.Start();
      spreadsheetWriter.Write();
      stopwatch.Stop();
      Console.WriteLine($"Done in {stopwatch.Elapsed.Seconds} seconds !");

      using FileStream file = new FileStream(Path.Combine(tempDi.FullName, "TestWithData3.xlsx"), FileMode.Create, FileAccess.Write);
      memoryStream.WriteTo(file);
    }

    private static void GenerateBigSpreadsheetFromWorkBookDfn(DirectoryInfo tempDi)
    {
      Console.WriteLine("GenerateBigSpreadsheetFromWorkBookDfn");
      using var memoryStream = new MemoryStream();
      using var streamWriter = new StreamWriter(memoryStream);
      var workbookDfn = new WorkbookDfn();
      var worksheetDfn = new WorksheetDfn("Team");
      workbookDfn.Worksheets.Add(worksheetDfn);
      Random rnd = new Random();
      WeakPseudoRandomDateTime weakPseudoRandomDate = new WeakPseudoRandomDateTime();
      Console.WriteLine("Generating the players...");
      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Start();
      for (int i = 0; i < 1000000; i++)
      {
        var rowDfn = new RowDfn
        {
          Cells =
          {
            new CellDfn($"Code{i}"),
            new CellDfn($"Player{i}"),
            new CellDfn(rnd.Next(18, 100), cellDataType: CellDataType.Number),
            new CellDfn(weakPseudoRandomDate.Next(), cellDataType: CellDataType.Date),
            new CellDfn(Convert.ToBoolean(rnd.Next(0, 100) % 2), cellDataType: CellDataType.Boolean),
            new CellDfn(rnd.Next(0, 100), cellDataType: CellDataType.Number),
            new CellDfn(Convert.ToDouble(rnd.Next(0, 100)) / 100, cellDataType: CellDataType.Percentage),
            new CellDfn(new TimeSpan(rnd.Next(0, 23), rnd.Next(0, 59), 0), cellDataType: CellDataType.Time),
          },
        };
        worksheetDfn.Rows.Add(rowDfn);
      }

      stopwatch.Stop();
      Console.WriteLine($"Done in {stopwatch.Elapsed.Seconds} seconds !");

      Console.WriteLine("Instantiating the SpreadsheetWriter...");
      stopwatch.Reset();
      stopwatch.Start();
      SpreadsheetWriter spreadsheetWriter = new SpreadsheetWriter(streamWriter.BaseStream, workbookDfn);
      stopwatch.Stop();
      Console.WriteLine($"Done in {stopwatch.Elapsed.Seconds} seconds !");

      Console.WriteLine("Writing the Excel file...");
      stopwatch.Reset();
      stopwatch.Start();
      spreadsheetWriter.Write();
      stopwatch.Stop();
      Console.WriteLine($"Done in {stopwatch.Elapsed.Seconds} seconds !");

      using FileStream file = new FileStream(Path.Combine(tempDi.FullName, "TestWithData4.xlsx"), FileMode.Create, FileAccess.Write);
      memoryStream.WriteTo(file);
    }

    private static void GenerateSpreadSheetFromAnnotatedData(DirectoryInfo tempDi)
    {
      Console.WriteLine("GenerateSpreadSheetFromAnnotatedData");
      using var memoryStream = new MemoryStream();
      using var streamWriter = new StreamWriter(memoryStream);
      var team = new Team
      {
        Players =
        {
          new Player
          {
            PlayerCode = null,
            PlayerName = "Alexandre",
            PracticeTime = new TimeSpan(9, 1, 0),
            Size = 1.93d,
            DateOfBirth = new DateTime(1974, 02, 01),
            IsActiveFlag = true,
            NumberOfVictory = 45,
            FieldGoalPercentage = 0.1111,
            Salary = 2000.5m,
            FemaleChildren = new List<Child>
             {
               new () { FirstName = "FirstName1", Age = 11 },
               new () { FirstName = "FirstName2", Age = 22 },
               new () { FirstName = "FirstName1", Age = 33 },
             },
            GamePlayed = 12,
            HeaderName0 = "abc",
          },
          new Player
          {
            PlayerCode = "02",
            PlayerName = "Elina",
            PracticeTime = new TimeSpan(9, 2, 0),
            Size = 1.72d,
            DateOfBirth = new DateTime(1990, 10, 13),
            IsActiveFlag = true,
            NumberOfVictory = 52,
            FieldGoalPercentage = 0.222,
            Salary = 2141.5452m,
            FemaleChildren = new List<Child>
             {
               new () { FirstName = "FirstName1", Age = 11 },
             },
            GamePlayed = 10,
            HeaderName0 = "abc",
          },
          new Player
          {
            PlayerCode = "03",
            PlayerName = "Franck",
            PracticeTime = new TimeSpan(9, 3, 0),
            Size = 1.85d,
            DateOfBirth = new DateTime(1976, 3, 1),
            IsActiveFlag = true,
            NumberOfVictory = 80,
            FieldGoalPercentage = 0.33,
            Salary = 2111.5452m,
            GamePlayed = 8,
            HeaderName0 = "abc",
          },
          new Player
          {
            PlayerCode = "04",
            PlayerName = "Yann",
            PracticeTime = new TimeSpan(9, 4, 0),
            Size = 1.79d,
            DateOfBirth = new DateTime(1979, 3, 1),
            IsActiveFlag = false,
            NumberOfVictory = 35,
            FieldGoalPercentage = 0.4,
            Salary = 2845.719m,
            GamePlayed = 6,
            HeaderName0 = "abc",
          },
        },
      };

      SpreadsheetWriter spreadsheetWriter = new SpreadsheetWriter(streamWriter.BaseStream, team);
      spreadsheetWriter.Write();

      using FileStream file = new FileStream(Path.Combine(tempDi.FullName, "TestWithData2.xlsx"), FileMode.Create, FileAccess.Write);
      memoryStream.WriteTo(file);
    }

    private static void GenerateSpreadSheetFromAnnotatedDataEmpty(DirectoryInfo tempDi)
    {
      Console.WriteLine("GenerateSpreadSheetFromAnnotatedDataEmpty");
      using var memoryStream = new MemoryStream();
      using var streamWriter = new StreamWriter(memoryStream);
      var team = new Team();

      SpreadsheetWriter spreadsheetWriter = new SpreadsheetWriter(streamWriter.BaseStream, team);
      spreadsheetWriter.Write();

      using FileStream file = new FileStream(Path.Combine(tempDi.FullName, "TestWithDataEmpty.xlsx"), FileMode.Create, FileAccess.Write);
      memoryStream.WriteTo(file);
    }

    private static void GenerateSpreadSheetFromWorkbookDfn(DirectoryInfo tempDi)
    {
      Console.WriteLine("GenerateSpreadSheetFromWorkbookDfn");
      using var memoryStream = new MemoryStream();
      using var streamWriter = new StreamWriter(memoryStream);
      var workbookDfn = new WorkbookDfn();

      // First sheet
      var worksheet1Dfn = new WorksheetDfn("MyFirstSheet");
      worksheet1Dfn.ColumnHeadings.Cells.Add(new CellDfn("Name|\b|\n|\t|\r|<|>|&|'|\"|"));
      worksheet1Dfn.ColumnHeadings.Cells.Add(new CellDfn("Age"));
      worksheet1Dfn.ColumnHeadings.Cells.Add(new CellDfn("Rate"));
      worksheet1Dfn.ColumnHeadings.Cells.Add(new CellDfn("Postal code"));
      worksheet1Dfn.ColumnHeadings.Cells.Add(new CellDfn("DateTime"));
      worksheet1Dfn.ColumnHeadings.Cells.Add(new CellDfn("Field goal percentage"));
      worksheet1Dfn.ColumnHeadings.Cells.Add(new CellDfn("Practice time"));
      workbookDfn.Worksheets.Add(worksheet1Dfn);
      var row1 = new RowDfn();
      row1.Cells.Add(new CellDfn("Eric", cellDataType: CellDataType.String));
      row1.Cells.Add(new CellDfn(50, cellDataType: CellDataType.Number));
      row1.Cells.Add(new CellDfn(45.00M, cellDataType: CellDataType.Number));
      row1.Cells.Add(new CellDfn("01090", cellDataType: CellDataType.String));
      row1.Cells.Add(new CellDfn(DateTime.Now, cellDataType: CellDataType.Date));
      row1.Cells.Add(new CellDfn(0.0111, cellDataType: CellDataType.Percentage));
      row1.Cells.Add(new CellDfn(new TimeSpan(8, 1, 0), cellDataType: CellDataType.Time));
      worksheet1Dfn.Rows.Add(row1);
      var row2 = new RowDfn();
      row2.Cells.Add(new CellDfn("Bob", cellDataType: CellDataType.String));
      row2.Cells.Add(new CellDfn(42, cellDataType: CellDataType.Number));
      row2.Cells.Add(new CellDfn(78.00M, cellDataType: CellDataType.Number));
      row2.Cells.Add(new CellDfn("01080", cellDataType: CellDataType.String));
      row2.Cells.Add(new CellDfn(DateTime.Now, cellDataType: CellDataType.Date));
      row2.Cells.Add(new CellDfn(0.0222, cellDataType: CellDataType.Percentage));
      row2.Cells.Add(new CellDfn(new TimeSpan(8, 1, 0), cellDataType: CellDataType.Time));
      worksheet1Dfn.Rows.Add(row2);

      // Second sheet
      var worksheet2Dfn = new WorksheetDfn("MySecondSheet");
      worksheet2Dfn.ColumnHeadings.Cells.Add(new CellDfn("Name"));
      worksheet2Dfn.ColumnHeadings.Cells.Add(new CellDfn("Age"));
      worksheet2Dfn.ColumnHeadings.Cells.Add(new CellDfn("Rate"));
      worksheet2Dfn.ColumnHeadings.Cells.Add(new CellDfn("Postal Code"));
      worksheet2Dfn.ColumnHeadings.Cells.Add(new CellDfn("Field goal percentage"));
      worksheet2Dfn.ColumnHeadings.Cells.Add(new CellDfn("Practice Time"));
      workbookDfn.Worksheets.Add(worksheet2Dfn);

      // Third sheet
      var worksheet3Dfn = new WorksheetDfn("MyThirdSheet");
      workbookDfn.Worksheets.Add(worksheet3Dfn);
      var row31 = new RowDfn();
      row31.Cells.Add(new CellDfn("Eric", cellDataType: CellDataType.String));
      row31.Cells.Add(new CellDfn(50, cellDataType: CellDataType.Number));
      row31.Cells.Add(new CellDfn(45.00M, cellDataType: CellDataType.Number));
      row31.Cells.Add(new CellDfn("01090", cellDataType: CellDataType.String));
      row31.Cells.Add(new CellDfn(DateTime.Now, cellDataType: CellDataType.Date));
      row31.Cells.Add(new CellDfn(true, cellDataType: CellDataType.Boolean));
      row31.Cells.Add(new CellDfn(0.11, cellDataType: CellDataType.Percentage));
      row31.Cells.Add(new CellDfn(new TimeSpan(8, 1, 0), cellDataType: CellDataType.Time));
      worksheet3Dfn.Rows.Add(row31);
      var row32 = new RowDfn();
      row32.Cells.Add(new CellDfn("Bob", cellDataType: CellDataType.String));
      row32.Cells.Add(new CellDfn(42, cellDataType: CellDataType.Number));
      row32.Cells.Add(new CellDfn(78.00M, cellDataType: CellDataType.Number));
      row32.Cells.Add(new CellDfn("01080", cellDataType: CellDataType.String));
      row32.Cells.Add(new CellDfn(DateTime.Now, cellDataType: CellDataType.Date));
      row32.Cells.Add(new CellDfn(false, cellDataType: CellDataType.Boolean));
      row32.Cells.Add(new CellDfn(22, cellDataType: CellDataType.Percentage));
      row32.Cells.Add(new CellDfn(new TimeSpan(8, 2, 0), cellDataType: CellDataType.Time));
      worksheet3Dfn.Rows.Add(row32);

      SpreadsheetWriter spreadsheetWriter = new SpreadsheetWriter(streamWriter.BaseStream, workbookDfn);
      spreadsheetWriter.Write();

      using FileStream file = new FileStream(Path.Combine(tempDi.FullName, "TestWithData1.xlsx"), FileMode.Create, FileAccess.Write);
      memoryStream.WriteTo(file);
    }
  }
}
