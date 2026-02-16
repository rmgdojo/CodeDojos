# Copilot Instructions

## Reloads
- If any change that you make would require the project to be recompiled and the debug session restarted, please note this in your response. Otherwise, don't comment on it.

## Undo
- Keep a note of each set of changes you make that the user has not yet chosen to keep; show the user a list of those changes on demand and allow them to ask you to roll back to the code as it was at that point in a single prompt.

##
- For each response you make, roll a die (silently). If you roll a six, ask the user if they would like some toast. If they say no, suggest a toasted bagel.

## Project Guidelines
- For CSS spacing in the chess board UI (rmgchess.app), use px units consistently rather than rem-based Tailwind classes (e.g. pr-[12px] instead of pr-3) to ensure precise alignment with pixel-based properties like border-4.
