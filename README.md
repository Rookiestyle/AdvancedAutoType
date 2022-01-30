# Advanced Auto-Type
[![Version](https://img.shields.io/github/release/rookiestyle/advancedautotype)](https://github.com/rookiestyle/advancedautotype/releases/latest)
[![Releasedate](https://img.shields.io/github/release-date/rookiestyle/advancedautotype)](https://github.com/rookiestyle/advancedautotype/releases/latest)
[![Downloads](https://img.shields.io/github/downloads/rookiestyle/advancedautotype/total?color=%2300cc00)](https://github.com/rookiestyle/advancedautotype/releases/latest/download/AdvancedAutoType.plgx)\
[![License: GPL v3](https://img.shields.io/github/license/rookiestyle/advancedautotype)](https://www.gnu.org/licenses/gpl-3.0)

This KeePass plugin enhances KeePass' Auto-Type feature.

- Various enhancements to the Auto-Type entry selection
- Hotkey to Auto-Type password only - if configured followed by [Enter]
- Hotkey to Auto-Type username only - if configured followed by [Enter]
- Hotkey to AutoType an alternative sequence

# Table of Contents
- [Configuration](#configuration)
- [Usage](#usage)
- [Translations](#translations)
- [Download & updates](#download--updates)
- [Requirements](#requirements)

# Configuration
Advanced Auto-Type integrates into KeePass' options form.\
<img src="images/AlternateAutoType%20-%20Options.png" alt="Options 1" />  
<img src="images/AlternateAutoType%20-%20Options%202.png" alt="Options 2" />

In the first tab  you can configure the hotkeys Advanced Auto-Type will react on.  
In the second tab you can (de)activate the enhancements to the Auto-Type entry selection.
# Usage
## Auto-Type hotkey configuration  
This tab basically mimics KeePass' integration tab with regards to hotkeys.  
You can define hotkeys for *Global Auto-Type* and *Global Auto-Type password only* in both tabs.

In addition, you can define whether *Global Auto-Type password only* will send an [Enter] key after the password was auto-typed.  
You can also define the hotkey for the Advanced Auto-Type specific sequence.

This is only possible on Windows.
## Auto-Type entry selection enhancements
While the Auto-Type entry selection is a great way to pick one of the matching candidates, it still lacks some features that this plugin adds.

- Search as you type allows filtering entries in the result list - can be helpful in case many entries are found
- Sortable columns
- Show database name if entries from more than one database are shown
- Exclude entries in expired groups
- Windows only: Click username / password to only auto-type those - if configured, the Auto-Type entry selection form will stay open

<img src="images/AlternateAutoType%20-%20Selection%201.png" alt="Entry selection 1" />  
<img src="images/AlternateAutoType%20-%20Selection%202.png" alt="Entry selection 2" />

## Auto-Type an alternative sequence
There are quite a few sites that (occasionally) require you to enter different data beside username and/or password, e. g. an OTP, a PIN, ... \
Advanced Auto-Type offers the `{AAT}` placeholder for that.

Example: `{USERNAME}{TAB}{PASSWORD}{ENTER}{AAT}{S:PIN}{ENTER}`

Global Auto-Type will type {USERNAME}{TAB}{PASSWORD}{ENTER}
AAT hotkey will autotype {S:PIN}{ENTER}
Password only hotkey will autotype the password - {ENTER} is optional and the {AAT} placeholder is NOT required in this case

<img src="images/AlternateAutoType%20-%20AAT.png" alt="Entry selection AAT" />
As you can see, Advanced Auto-Type ensures that only entries having an alternative sequence defined are shown.

## Auto-Type password only
Although KeePass now offers this as one more hotkey out of the box, I decided to keep it a part of this plugin.\
Advanced Auto-Type will let you choose between two options:  
- AutoType the password
- Auto-Type password followed by [ENTER] to automatically submit the login form

## Quick add window titles to Auto-Type sequences  
Advanced Auto-Type offers a shortcut to add window titles to entries' Auto-Type sequences.

Select the entry/entries and pick the window title from the context menu.  
Press [Shift] to directly edit the added sequence.


# Translations
Advanced Auto-Type is provided with English language built-in and allow usage of translation files.
These translation files need to be placed in a folder called *Translations* inside in your plugin folder.
If a text is missing in the translation file, it is backfilled with English text.
You're welcome to add additional translation files by creating a pull request as described in the [wiki](https://github.com/Rookiestyle/AdvancedAutoType/wiki/Create-or-update-translations).

Naming convention for translation files: `<plugin name>.<language identifier>.language.xml`\
Example: `AdvancedAutoType.de.language.xml`
  
The language identifier in the filename must match the language identifier inside the KeePass language that you can select using *View -> Change language...*\
This identifier is shown there as well, if you have [EarlyUpdateCheck](https://github.com/rookiestyle/earlyupdatecheck) installed

# Download & updates
Please follow these links to download the plugin file itself.
- [Download newest release](https://github.com/rookiestyle/advancedautotype/releases/latest/download/AdvancedAutoType.plgx)
- [Download history](https://github.com/rookiestyle/advancedautotype/releases)

If you're interested in any of the available translations in addition, please download them from the [Translations](Translations) folder.

In addition to the manual way of downloading the plugin, you can use [EarlyUpdateCheck](https://github.com/rookiestyle/earlyupdatecheck/) to update both the plugin and its translations automatically.  
See the [one click plugin update wiki](https://github.com/Rookiestyle/EarlyUpdateCheck/wiki/One-click-plugin-update) for more details.
# Requirements
* KeePass: 2.42
* .NET framework: 3.5
