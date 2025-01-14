﻿namespace ConsoleApp
{
  using System;
  using System.Collections.Generic;
  using SimpleExcelExporter.Annotations;
  using SimpleExcelExporter.Definitions;

  public class Player
  {
    [CellDefinition(CellDataType.Date)]
    [Header(typeof(PlayerRes), "DateOfBirthColumnName")]
    [Index(2)]
    public DateTime? DateOfBirth { get; set; }

    [CellDefinition(CellDataType.String)]
    [Header(typeof(PlayerRes), "PlayerCodeColumnName")]
    [Index(0)]
    public string? PlayerCode { get; set; }

    [CellDefinition(CellDataType.String)]
    [Header(typeof(PlayerRes), "PlayerNameColumnName")]
    [Index(1)]
    public string? PlayerName { get; set; }

    [CellDefinition(CellDataType.Time)]
    [Header(typeof(PlayerRes), "PracticeTimeColumnName")]
    [Index(8)]
    public TimeSpan? PracticeTime { get; set; }

    [CellDefinition(CellDataType.Number)]
    [Header(typeof(PlayerRes), "NumberOfVictoryColumnName")]
    [Index(3)]
    public int? NumberOfVictory { get; set; }

    [CellDefinition(CellDataType.Boolean)]
    [Header(typeof(PlayerRes), "IsActiveFlagColumnName")]
    [Index(4)]
    public bool? IsActiveFlag { get; set; }

    [CellDefinition(CellDataType.Percentage)]
    [Header(typeof(PlayerRes), "FieldGoalPercentageColumnName")]
    [Index(5)]
    public double? FieldGoalPercentage { get; set; }

    [CellDefinition(CellDataType.Boolean)]
    [Header(typeof(PlayerRes), "SizeColumnName")]
    [Index(6)]
    public double? Size { get; set; }

    [CellDefinition(CellDataType.Number)]
    [Header(typeof(PlayerRes), "SalaryColumnName", nameof(HeaderName0))]
    [Index(7)]
    public decimal? Salary { get; set; }

    [MultiColumn]
    [Index(9)]
    public ICollection<Child>? MaleChildren { get; set; }

    [MultiColumn]
    [Index(10)]
    public ICollection<Child>? FemaleChildren { get; set; }

    [CellDefinition(CellDataType.Number)]
    [Header(typeof(PlayerRes), "GamePlayedColumnName")]
    [Index(11)]
    public decimal? GamePlayed { get; set; }

    [IgnoreFromSpreadSheet]
    public string? HeaderName0 { get; set; }

    [IgnoreFromSpreadSheet]
    public string? HeaderName1 { get; set; }
  }
}
