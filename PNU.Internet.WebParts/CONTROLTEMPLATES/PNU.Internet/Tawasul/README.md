# Tawasul (تواصل / Contact) — SharePoint 2019 farm-solution controls

Server-side user controls for the University **Contact / تواصل** page, built to the same
architecture as the **NouraStudents** and **International** solutions.

**Namespace:** `PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Tawasul.Controls`
**Deploy path:** `CONTROLTEMPLATES\PNU.Internet\Tawasul\` (i.e. `~/_controltemplates/15/PNU.Internet/Tawasul/...`)
**Content web:** `/ar/Tawasul`  •  **AdminUsers web:** `/ar/ContentAdmin` (shared with the other pages)

## What it renders

1. **التواصل مع الجامعة** + **تواصل نورة** — the two top cards (`ucTwContactCards`).
2. **الجهات داخل الجامعة** — the internal-entities grid (`ucTwEntities`).
3. **الموقع** — national address + embedded map (`ucTwLocation`).

Drop the container onto a page layout:

```aspx
<%@ Register TagPrefix="pnu" TagName="Tawasul"
    Src="~/_controltemplates/15/PNU.Internet/Tawasul/Controls/ucTawasul.ascx" %>
...
<pnu:Tawasul runat="server" />           <%-- ShowContactCards / ShowEntities / ShowLocation = "False" to hide --%>
```

The admin screen (its own page/web part zone):

```aspx
<%@ Register TagPrefix="pnu" TagName="TawasulAdmin"
    Src="~/_controltemplates/15/PNU.Internet/Tawasul/Admin/ucTwAdmin.ascx" %>
...
<pnu:TawasulAdmin runat="server" />       <%-- add FixedListName="TwEntities" to lock it to one list --%>
```

## Lists (auto-provisioned + seeded on first page load and on feature activation)

All live on `/ar/Tawasul`:

| List | Purpose |
|------|---------|
| `TwSectionTitles`   | **Every section heading + subtitle** (SectionKey: UniversityContact, TawasulNourah, Entities, Location, DigitalPlatforms) |
| `TwContactChannels` | Phone / e-mail / link rows of the two top cards, grouped by `ContactGroup` |
| `TwSocialLinks`     | Digital platforms (X, Instagram, LinkedIn, YouTube) |
| `TwEntities`        | Internal university entities grid |
| `TwLocation`        | National address, short code (RUKA7808) and map embed — one item, reused by the University card and the Location section |

`AdminUsers` (Person column `UserAccount` + `Active`) is provisioned on `/ar/ContentAdmin`.

## Access model (identical to NouraStudents)

`ucTwAdmin` grants access **only** to users with an active row in the `AdminUsers` list on
`/ar/ContentAdmin` — no site-collection admin / ManageLists shortcut. Matching is by the
Person field, checked by lookup-id and by display name in one elevated CAML query.
Until `/ar/ContentAdmin` exists and has at least one active row, nobody can open the admin
screen; create the web, load any page once to provision the list, then add your login via
`/ar/ContentAdmin/Lists/AdminUsers`.

## Notes

- The subweb `/ar/Tawasul` must exist (the code will not create it); otherwise everything
  falls back to the current web and a "Target web … was not found" line is logged.
- Every string is HTML-encoded at projection time, so the ASCX binds raw with `Eval(...)`.
- `TwLog` is the only file that references the portal `Publics.WriteToLog` logger — fix the
  `using` there once if it does not resolve.
