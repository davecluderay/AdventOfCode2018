using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text.RegularExpressions;

namespace Aoc2018_Day04
{
    internal class Solution
    {
        public string Title => "Day 4: Repose Record";

        public object? PartOne()
        {
            var data = ReadAsleepData();
            var sleepiestGuard = data.OrderByDescending(g => g.AsleepMinutes.Sum(a => a.Frequency)).First();
            var minuteMostOftenSleeping = sleepiestGuard.AsleepMinutes.OrderByDescending(a => a.Frequency).First();
            return sleepiestGuard.Guard * minuteMostOftenSleeping.Minute; // 39422
        }

        public object? PartTwo()
        {
            var data = ReadAsleepData();
            var selection = data.Select(x => (x.Guard,
                                              MinuteMostOftenSleeping: x.AsleepMinutes.OrderByDescending(a => a.Frequency)
                                                                                      .First()))
                                .OrderByDescending(x => x.MinuteMostOftenSleeping.Frequency)
                                .First();
            return selection.Guard * selection.MinuteMostOftenSleeping.Minute; // 65474
        }

        private record AsleepMinuteFrequency(int Minute, int Frequency);

        private static (int Guard, AsleepMinuteFrequency[] AsleepMinutes)[] ReadAsleepData()
        {
            var entries = InputFile.ReadAllLines()
                                   .Select(ParseLogEntry)
                                   .OrderBy(e => (e.Date, e.Time));

            var napsByGuard = new ConcurrentDictionary<int, ConcurrentDictionary<int, int>>();
            int? currentGuard = null;
            TimeSpan? currentNapStart = null;
            foreach (var entry in entries)
            {
                if (entry.Guard is null && currentGuard is null) throw new Exception("Unknown guard.");
                switch (entry.Event)
                {
                    case "begins shift":
                        currentGuard = entry.Guard;
                        currentNapStart = null;
                        break;
                    case "falls asleep":
                        currentNapStart = entry.Time;
                        break;
                    case "wakes up":
                    {
                        var guard = currentGuard!.Value;

                        var frequenciesByMinute = napsByGuard.GetOrAdd(guard, _ => new ConcurrentDictionary<int, int>());

                        for (var minute = (int)currentNapStart!.Value.TotalMinutes; minute < entry.Time.TotalMinutes; minute++)
                            frequenciesByMinute.AddOrUpdate(minute, _ => 1, (_, v) => v + 1);

                        currentNapStart = null;
                        break;
                    }
                }
            }

            return napsByGuard.Select(x => (Guard: x.Key,
                                            AsleepMinutes: x.Value.Select(a => new AsleepMinuteFrequency(a.Key, a.Value))
                                                                  .OrderBy(a => a.Minute).ToArray()))
                              .OrderBy(x => x.Guard)
                              .ToArray();
        }

        private static readonly Regex LogEntryPattern = new (@"^\[(?'yyyy'\d{4})-(?'MM'\d{2})-(?'dd'\d{2}) (?'HH'\d{2}):(?'mm'\d{2})\]( Guard #(?'Guard'\d+))? (?'Action'begins shift|wakes up|falls asleep)$",
                                                             RegexOptions.Compiled | RegexOptions.ExplicitCapture);

        private record LogEntry(DateOnly Date, TimeSpan Time, int? Guard, string Event);

        private static LogEntry ParseLogEntry(string text)
        {
            var match = LogEntryPattern.Match(text);
            var date = new DateOnly(Convert.ToInt32(match.Groups["yyyy"].Value),
                                    Convert.ToInt32(match.Groups["MM"].Value),
                                    Convert.ToInt32(match.Groups["dd"].Value));
            var time = TimeSpan.FromHours(Convert.ToInt32(match.Groups["HH"].Value))
                               .Add(TimeSpan.FromMinutes(Convert.ToInt32(match.Groups["mm"].Value)));
            var guard = int.TryParse(match.Groups["Guard"].Value, out var v) ? (int?)v : null;
            var action = match.Groups["Action"].Value;
            return new LogEntry(date, time, guard, action);
        }
    }
}
