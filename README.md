# epi-epiphan-pearl
<!-- START Minimum Essentials Framework Versions -->
### Minimum Essentials Framework Versions

- 3.0.0
<!-- END Minimum Essentials Framework Versions -->
<!-- START Supported Types -->

<!-- END Supported Types -->
<!-- START Join Maps -->
### Join Maps

#### Digitals

| Join | Type (RW) | Description |
| --- | --- | --- |
| 1 | R | Recorder Online |
| 2 | R | Start Recording |
| 3 | R | Stop Recording |
| 4 | R | Pause Recording |
| 5 | R | Resume Recording |
| 6 | R | Extend Recording |
| 5 | R | Recording Is Paused |
| 6 | R | Recording Is In Progress |
| 20 | R | Next Recording Exists |

#### Serials

| Join | Type (RW) | Description |
| --- | --- | --- |
| 1 | R | Set Hostname |
| 11 | R | CurrentRecordingId |
| 12 | R | Recorder Name |
| 13 | R | CurrentRecordingStartTime |
| 14 | R | CurrentRecordingEndTime |
| 15 | R | CurrentRecordingLength |
| 16 | R | CurrentRecordingTimeRemaining |
| 21 | R | NextRecordingId |
| 22 | R | Recorder Name |
| 23 | R | NextRecordingStartTime |
| 24 | R | NextRecordingEndTime |
| 25 | R | NextRecordingLength |
<!-- END Join Maps -->
<!-- START Interfaces Implemented -->
### Interfaces Implemented

- IEpiphanPearlClient
- ICommunicationMonitor
- IHasRecordingPause
- IHasRecordingSchedule
- IHasPolling
<!-- END Interfaces Implemented -->
<!-- START Base Classes -->
### Base Classes

- DateTimeConverterBase
- ReconfigurableBridgableDevice
- JoinMapBaseAdvanced
- StatusMonitorBase
<!-- END Base Classes -->
<!-- START Public Methods -->
### Public Methods

- public void SetHost(string host)
- public string Delete(string path)
- public void Poll()
- public void PauseRecording()
- public void ResumeRecording()
- public void PauseRunningEvent()
- public void ResumeRunningEvent()
- public void StopRunningEvent()
- public void StartEvent()
- public void ExtendRunningEvent(int minutes)
- public void SetIpAddress(string hostname)
- public void SetOnlineStatus(bool isOnline)
<!-- END Public Methods -->

<!-- START Bool Feedbacks -->

<!-- END Bool Feedbacks -->
<!-- START Int Feedbacks -->

<!-- END Int Feedbacks -->
<!-- START String Feedbacks -->
### String Feedbacks

- NameFeedback
- IdFeedback
- StartFeedback
- EndFeedback
- LengthFeedback
<!-- END String Feedbacks -->
