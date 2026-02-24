# epi-epiphan-pearl
<!-- START Minimum Essentials Framework Versions -->
### Minimum Essentials Framework Versions

- 1.12.8
<!-- END Minimum Essentials Framework Versions -->
<!-- START Supported Types -->

<!-- END Supported Types -->
<!-- START Join Maps -->
### Join Maps

#### Digitals

| Join | Type (RW) | Description |
| --- | --- | --- |
| 1 | R | Recorder Online (ToSIMPL feedback) |
| 2 | W | Start Recording (FromSIMPL command) |
| 3 | W | Stop Recording (FromSIMPL command) |
| 4 | W | Pause Recording (FromSIMPL command) |
| 5 | W | Resume Recording (FromSIMPL command) |
| 6 | W | Extend Recording (FromSIMPL command) |
| 5 | R | Recording Is Paused (ToSIMPL feedback for join 5) |
| 6 | R | Recording Is In Progress (ToSIMPL feedback for join 6) |
| 20 | R | Next Recording Exists (ToSIMPL feedback) |

#### Serials

| Join | Type (RW) | Description |
| --- | --- | --- |
| 1 | W | Set Hostname (FromSIMPL command) |
| 11 | R | CurrentRecordingId (ToSIMPL feedback) |
| 12 | R | Recorder Name (current, ToSIMPL feedback) |
| 13 | R | CurrentRecordingStartTime (ToSIMPL feedback) |
| 14 | R | CurrentRecordingEndTime (ToSIMPL feedback) |
| 15 | R | CurrentRecordingLength (ToSIMPL feedback) |
| 16 | R | CurrentRecordingTimeRemaining (ToSIMPL feedback) |
| 21 | R | NextRecordingId (ToSIMPL feedback) |
| 22 | R | Recorder Name (next, ToSIMPL feedback) |
| 23 | R | NextRecordingStartTime (ToSIMPL feedback) |
| 24 | R | NextRecordingEndTime (ToSIMPL feedback) |
| 25 | R | NextRecordingLength (ToSIMPL feedback) |
<!-- END Join Maps -->
<!-- START Interfaces Implemented -->
### Interfaces Implemented

- IEpiphanPearlClient
- ICommunicationMonitor
<!-- END Interfaces Implemented -->
<!-- START Base Classes -->
### Base Classes

- ReconfigurableBridgableDevice
- DateTimeConverterBase
- JoinMapBaseAdvanced
- StatusMonitorBase
<!-- END Base Classes -->
<!-- START Public Methods -->
### Public Methods

- public string Delete(string path)
- public void setHost(string host)
- public void SetIpAddress(string hostname)
- public void SetOnlineStatus(bool isOnline)
- public void UpdateTimers()
<!-- END Public Methods -->

