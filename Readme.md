# Azure Log Viewer

## Worker

Worker will dump events and send then to slack.

To run in a 'Single Dump' mode (suitable for cron jobs) use `LogViewer:SingleDump` parameter set to `true`.

E.g. in comand line aqrguments:

```cmd
LogAnalyticsViewer.Worker.exe /LogViewer:SingleDump=true
```