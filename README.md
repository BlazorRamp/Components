# Blazor Ramp … in progress …

Blazor Ramp aims to provide a suite of modular, accessible-first Blazor components, delivered individually via NuGet.

Please check back **regularly** during this **initial** phase whilst I put in place everything you would expect.

Whilst you are waiting, please visit the test site [https://blazorramp.uk](https://blazorramp.uk)
 and use any **assistive** technology at your disposal to help identify any issues with the **Live Region Service**, **Announcement history** and **Busy Indicator** components under test.

Currently I am putting the finishing touches on the Core project prior to release as well making start on the documentation site that will be for
all the components. This will be a separate Blazor WASM site (again hosted on GitHub Pages) using: [https://docs.blazorramp.uk](https://docs.blazorramp.uk). I will most likely release the Core project prior to finishing all of the documentation so people can take it for a spin,
as the main / longest part of the documentation will be regarding all of the css properties and the implemented hierarchy.

**Screen Reader Browser Combination Tests:** 
- On Windows 11 - JAWS, NVDA and Narrator each paired with Chrome, Edge and FireFox.
- On macOS (Sequoia) VoiceOver paired with Safari
- On iPhone, VoiceOver paired with Safari
- On Android, TalkBack paired with Chrome

VO on macOs/IPhone has a minor edge case problem (when making multiple rapid announcements) that is not worth looking at (IMHO).
I changed the code so when making elements inert it does not make the triggering button inert, so the focus remains put unless intentionally moved. This was needed to 
for VO on macOS.

Thank you for **your** patience,

Paul

**P.S** I need a logo and icon for the NuGet packages if you have a spare one
