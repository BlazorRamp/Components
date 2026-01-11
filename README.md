# Blazor Ramp … in progress …

Blazor Ramp aims to provide a suite of modular, accessible-first Blazor components, delivered individually via NuGet.

Please check back **regularly** during this **initial** phase whilst I put in place everything you would expect.

Whilst you are waiting, please visit the test site [blazorramp.uk](https://blazorramp.uk)
 and use any **assistive** technology at your disposal to help identify any issues with the **Live Region Service**, **Announcement history** and **Busy Indicator** components under test.

Thank you for **your** patience,

**Update:** I can now test locally with macOS Sequoia so I can include VoiceOver (VO) on Safari as a testable combination. 
Talkback on Android with Chrome appears fine. VO on macOs/IPhone has a minor edge case problem that is not worth looking at (IMHO).
I changed the code so when making elements inert it does not make the triggering button inert, so the focus remains put unless intentionally moved. I had
to do this for VO on macOS.

Paul

**P.S** I need a logo and icon for the NuGet packages if you have a spare one
