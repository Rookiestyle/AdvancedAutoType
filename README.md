# Alternate Auto-Type
[![Version](https://img.shields.io/github/release/rookiestyle/alternateautotype)](https://github.com/rookiestyle/alternateautotype/releases/latest)
[![Releasedate](https://img.shields.io/github/release-date/rookiestyle/alternateautotype)](https://github.com/rookiestyle/alternateautotype/releases/latest)
[![Downloads](https://img.shields.io/github/downloads/rookiestyle/alternateautotype/total?color=%2300cc00)](https://github.com/rookiestyle/alternateautotype/releases/latest/download/AlternateAutoType.plgx)\
[![License: GPL v3](https://img.shields.io/github/license/rookiestyle/alternateautotype)](https://www.gnu.org/licenses/gpl-3.0)

This KeePass plugin enhances KeePass' Auto-Type feature.

- Various enhancements to the Auto-Type entry selection
- Hotkey to Auto-Type password only - if configured followed by [Enter]
- Hotkey to AutoType an alternative sequence

# Table of Contents
- [Configuration](#configuration)
- [Usage](#usage)
- [Translations](#translations)
- [Download and Requirements](#download-and-requirements)

# Configuration
Alternate Auto-Type integrates into KeePass' options form.\
<img src="images/AlternateAutoType%20-%20Options.png" alt="Options" />

In the upper area you can configure the hotkeys Alternate Auto-Type will react on.  
In the lower area you can (de)activate the enhancements to the Auto-Type entry selection.
# Usage
## Auto-Type entry selection enhancements
While the Auto-Type entry selection is a great way to pick one of the matching candidates, it still lacks some features that this plugin adds.

- Sortable columns
- Show database name if entries from more than one database are shown
- Exclude entries in expired groups
- Windows only: Click username / password to only auto-type those - if configured, the Auto-Type entry selection form will stay open

<img src="images/AlternateAutoType%20-%20Selection%201.png" alt="Entry selection 1" />  
<img src="images/AlternateAutoType%20-%20Selection%202.png" alt="Entry selection 2" />

## Auto-Type an alternative sequence
There are quite a few sites that (occasionally) require you to enter different data beside username and/or password, e. g. an OTP, a PIN, ... \
Alternate Auto-Type offers the `{AAT}` placeholder for that.

Example: `{USERNAME}{TAB}{PASSWORD}{ENTER}{AAT}{S:PIN}{ENTER}`

Global Auto-Type will type {USERNAME}{TAB}{PASSWORD}{ENTER}
AAT hotkey will autotype {S:PIN}{ENTER}
Password only hotkey will autotype the password - {ENTER} is optional and the {AAT} placeholder is NOT required in this case

<img src="images/AlternateAutoType%20-%20AAT.png" alt="Entry selection AAT" />
As you can see, ALternateAutoType ensures that only entries having an alternative sequence defined are shown.

## Auto-Type password only
Although KeePass now offers this as onemore hotkey out of the box, I decided to keep it a part of this plugin.\
Alternate Auto-Type will let you chose between two options:  
- AutoType the password
- Auto-Type password foloowed by [ENTER] to automatically submit the login form

# Translations
Alternate Auto-Type is provided with English language built-in and allow usage of translation files.
These translation files need to be placed in a folder called *Translations* inside in your plugin folder.
If a text is missing in the translation file, it is backfilled with English text.
You're welcome to add additional translation files by creating a pull request as described in the [wiki](https://github.com/Rookiestyle/AlternateAutoType/wiki/Create-or-update-translations).

Naming convention for translation files: `AlternateAutoType.<language identifier>.language.xml`\
Example: `AlternateAutoType.de.language.xml`
  
The language identifier in the filename must match the language identifier inside the KeePass language that you can select using *View -> Change language...*\
This identifier is shown there as well, if you have [EarlyUpdateCheck](https://github.com/rookiestyle/earlyupdatecheck) installed

# Download and Requirements
## Download
Please follow these links to download the plugin file itself.
- [Download newest release](https://github.com/rookiestyle/alternateautotype/releases/latest/download/AlternateAutoType.plgx)
- [Download history](https://github.com/rookiestyle/alternateautotype/releases)

If you're interested in any of the available translations in addition, please download them from the [Translations](Translations) folder.
## Requirements
* KeePass: 2.42

