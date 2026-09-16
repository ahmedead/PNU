<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucOrgStructure.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.About.ucOrgStructure" %>
<%--
    ucOrgStructure — PNU organizational-structure chart (DGA / bilingual).
    The SVG is a label-stripped "skeleton": it draws the boxes, connector lines and
    dots, but NO text. Every label is rendered as an HTML element in an overlay layer,
    positioned over its box from the OrgStructureUnits list (Title / TitleEn), so the
    chart is fully bilingual and editable — no baked-in Arabic text.
--%>
<section class="py-5" data-aos="fade-up">
    <div class="container">
        <%-- Ensures the Hugeicons stylesheet is present so box icons render as glyphs
             (not as "hgi-..." class text) even if the master page doesn't load it. --%>
        <asp:Literal ID="litIconCss" runat="server" EnableViewState="false" />
        <style>
            .org-box-label::before {content: none !important; }
            .org-structure-stage {
                position: relative;
                isolation: isolate;
                container-type: inline-size;
            }
            .org-structure-svg { width: 100%; height: auto; display: block; }

            /* Guard: the master page / global CSS must never inject pseudo-elements
               onto the skeleton SVG (they leave stray marks/boxes on the chart). */
            .org-structure-svg::before,
            .org-structure-svg::after,
            .org-structure-svg *::before,
            .org-structure-svg *::after { content: none !important; }

            /* Overlay label layer -------------------------------------------------- */
            .org-overlay { position: absolute; inset: 0; pointer-events: none; }
            .org-box-label {
                position: absolute;
                display: flex; flex-direction: row; align-items: center; justify-content: center;
                gap: 0.45cqw;
                text-align: center;
                box-sizing: border-box;
                padding: 0 0.5cqw;
                font-weight: 700;
                line-height: 1.18;
                font-size: clamp(9px, 1.02cqw, 15px);
                cursor: default;
                pointer-events: auto;
                transition: transform 0.18s ease, filter 0.18s ease;
            }
            .org-box-label .obl-icon { flex: 0 0 auto; font-size: 1.2em; line-height: 1; }
            .org-box-label .obl-icon:empty { display: none; }
            .org-box-label .obl-text {
                display: -webkit-box;
                -webkit-line-clamp: 3;
                -webkit-box-orient: vertical;
                overflow: hidden;
            }

            /* Content is centered in every box; direction is set per language so the
               icon sits on the correct (leading) side of the text and Arabic shapes right. */
            .org-rtl .org-box-label { direction: rtl; justify-content: center; text-align: center; }
            .org-ltr .org-box-label { direction: ltr; justify-content: center; text-align: center; }

            /* Legend (color key) labels — text hugs the swatch column (physical right). */
            .org-legend-label {
                position: absolute;
                display: flex; flex-direction: column; justify-content: center;
                text-align: right;
                box-sizing: border-box;
                color: var(--dga-primary-950);
                font-weight: 600;
                line-height: 1.1;
                font-size: clamp(7px, 0.8cqw, 12px);
                pointer-events: none;
            }
            .org-rtl .org-legend-label { direction: rtl; }
            .org-ltr .org-legend-label { direction: ltr; }
            .org-box-label.has-link { cursor: pointer; }
            @media (hover: hover) and (pointer: fine) {
                .org-box-label.has-link:hover,
                .org-box-label.is-active { filter: drop-shadow(0 8px 14px color-mix(in srgb, var(--dga-primary-950) 22%, transparent)); }
                .org-box-label.has-link:hover { transform: translateY(-1px); }
            }
            .org-box-label:focus-visible { outline: 2px solid var(--dga-primary); outline-offset: 2px; border-radius: 6px; }

            /* Tooltip card --------------------------------------------------------- */
            .org-unit-tooltip {
                position: fixed; top: 0; left: 0;
                width: min(22rem, calc(100vw - 1.5rem));
                pointer-events: none; opacity: 0;
                transform: translate3d(0, 0.75rem, 0) scale(0.98);
                transform-origin: top right;
                transition: opacity 0.18s ease, transform 0.18s ease;
                z-index: 20;
            }
            .org-unit-tooltip.is-visible { opacity: 1; transform: translate3d(0,0,0) scale(1); pointer-events: auto; }
            /* The details arrow points to the reading direction: left for Arabic (RTL),
               right for English (LTR). The markup uses a left arrow, so mirror it in LTR. */
            .org-ltr .org-unit-tooltip .hgi-arrow-left-02 { display: inline-block; transform: scaleX(-1); }
            @media (prefers-reduced-motion: reduce) {
                .org-box-label, .org-unit-tooltip { transition: none; }
            }
        </style>

        <asp:Panel ID="pnlStage" runat="server" CssClass="org-structure-stage">
                <svg class="org-structure-svg" viewBox="0 0 1237 1650" fill="none" xmlns="http://www.w3.org/2000/svg">
                    <rect x="0.75" y="0.75" width="188.25" height="232.5" rx="6.75" fill="var(--dga-primary-25)" stroke="var(--dga-primary-200)"
                        stroke-width="1.5" stroke-dasharray="1.5 1.5" />
                    <rect x="482.5" y="79.5" width="280" height="56" rx="8" stroke="var(--dga-primary-400)" stroke-width="4" />
                    
                    <rect x="477.5" y="170.5" width="290" height="56" rx="8" stroke="var(--dga-primary-700)" />
                    <rect x="482.5" y="165.5" width="280" height="56" rx="8" fill="var(--dga-primary-700)" />
                    
                    <rect x="632.5" y="234.5" width="135" height="40" rx="8" fill="var(--dga-primary-50)" />
                    
                    <rect x="477.5" y="234.5" width="135" height="40" rx="8" fill="var(--dga-primary-50)" />
                    
                    <rect x="475" y="199" width="74" height="1" rx="0.5" transform="rotate(-180 475 199)"
                        stroke="var(--dga-primary-400)" stroke-dasharray="2 2" />
                    <rect x="401" y="229" width="31" height="1" rx="0.5" transform="rotate(-90 401 229)"
                        stroke="var(--dga-primary-400)" stroke-dasharray="2 2" />
                    <circle cx="401.5" cy="229.5" r="3" transform="rotate(-90 401.5 229.5)" fill="var(--dga-primary-400)" />
                    <rect x="402" y="168" width="31" height="1" rx="0.5" transform="rotate(90 402 168)" stroke="var(--dga-primary-400)"
                        stroke-dasharray="2 2" />
                    <circle cx="401.5" cy="167.5" r="3" transform="rotate(90 401.5 167.5)" fill="var(--dga-primary-400)" />
                    <rect x="255.5" y="141.5" width="135" height="52" rx="8" stroke="var(--dga-primary-400)" />
                    
                    <rect x="255.5" y="203.5" width="135" height="52" rx="8" stroke="var(--dga-primary-400)" />
                    
                    <rect x="852.5" y="172.5" width="135" height="52" rx="8" fill="var(--dga-primary-400)" />
                    
                    <rect x="946" y="1391" width="160" height="1" rx="0.5" transform="rotate(-90 946 1391)"
                        fill="var(--dga-primary-900)" stroke="var(--dga-primary-200)" />
                    <rect x="864.5" y="845.5" width="163" height="52" rx="8" fill="var(--dga-primary-400)" />
                    
                    <rect x="864.5" y="909.5" width="163" height="52" rx="8" fill="var(--dga-primary-400)" />
                    
                    <rect x="1073.5" y="653.5" width="163" height="52" rx="8" fill="var(--dga-primary-400)" />
                    
                    <rect x="864.5" y="973.5" width="163" height="52" rx="8" fill="var(--dga-primary-400)" />
                    
                    <rect x="1073.5" y="717.5" width="163" height="52" rx="8" fill="var(--dga-primary-400)" />
                    
                    <rect x="1073.5" y="589.5" width="163" height="52" rx="8" fill="var(--dga-primary-400)" />
                    
                    <rect x="864.5" y="1101.5" width="163" height="52" rx="8" fill="var(--dga-primary-400)" />
                    
                    <rect x="1073.5" y="781.5" width="163" height="52" rx="8" fill="var(--dga-primary-400)" />
                    
                    <rect x="864.5" y="1037.5" width="163" height="52" rx="8" fill="var(--dga-primary-400)" />
                    
                    <rect x="864.5" y="781.5" width="163" height="52" rx="8" fill="var(--dga-primary-100)" />
                    
                    <rect x="864.5" y="1165.5" width="163" height="52" rx="8" stroke="var(--dga-primary-200)" />
                    
                    <rect x="864.5" y="589.5" width="163" height="52" rx="8" fill="var(--dga-primary-500)" />
                    
                    <rect x="864.5" y="717.5" width="163" height="52" rx="8" fill="var(--dga-primary-500)" />
                    
                    <rect x="655.5" y="781.5" width="163" height="52" rx="8" fill="var(--dga-primary-50)" />
                    
                    <rect x="655.5" y="845.5" width="163" height="52" rx="8" fill="var(--dga-primary-50)" />
                    
                    <rect x="655.5" y="909.5" width="163" height="52" rx="8" fill="var(--dga-primary-50)" />
                    
                    <rect x="655.5" y="973.5" width="163" height="52" rx="8" fill="var(--dga-primary-50)" />
                    
                    <rect x="655.5" y="1037.5" width="163" height="52" rx="8" fill="var(--dga-primary-50)" />
                    
                    <rect x="655.5" y="1101.5" width="163" height="52" rx="8" fill="var(--dga-primary-50)" />
                    
                    <rect x="655.5" y="1229.5" width="163" height="52" rx="8" fill="var(--dga-primary-50)" />
                    
                    <rect x="655.5" y="1165.5" width="163" height="52" rx="8" fill="var(--dga-primary-50)" />
                    
                    <rect x="864.5" y="653.5" width="163" height="52" rx="8" fill="var(--dga-primary-500)" />
                    
                    <rect x="1073.5" y="845.5" width="163" height="52" rx="8" fill="var(--dga-primary-400)" />
                    
                    <rect x="1073.5" y="1037.5" width="163" height="52" rx="8" fill="var(--dga-primary-400)" />
                    
                    <rect x="1073.5" y="973.5" width="163" height="52" rx="8" fill="var(--dga-primary-400)" />
                    
                    <rect x="1073.5" y="909.5" width="163" height="52" rx="8" fill="var(--dga-primary-400)" />
                    
                    <rect x="864.5" y="493.5" width="163" height="80" rx="8" fill="var(--dga-primary-700)" />
                    
                    <rect x="1073.5" y="493.5" width="163" height="80" rx="8" fill="var(--dga-primary-700)" />
                    
                    <rect x="1.5" y="589.5" width="163" height="52" rx="8" fill="var(--dga-primary-400)" />
                    
                    <rect x="1.5" y="653.5" width="163" height="52" rx="8" fill="var(--dga-primary-400)" />
                    
                    <rect x="1.5" y="717.5" width="163" height="52" rx="8" fill="var(--dga-primary-400)" />
                    
                    <rect x="1.5" y="781.5" width="163" height="52" rx="8" fill="var(--dga-primary-400)" />
                    
                    <rect x="1.5" y="845.5" width="163" height="52" rx="8" fill="var(--dga-primary-400)" />
                    
                    <rect x="1.5" y="493.5" width="163" height="80" rx="8" fill="var(--dga-primary-700)" />
                    
                    <rect x="216.5" y="589.5" width="163" height="52" rx="8" fill="var(--dga-primary-500)" />
                    
                    <rect x="216.5" y="653.5" width="163" height="52" rx="8" fill="var(--dga-primary-500)" />
                    
                    <rect x="216.5" y="781.5" width="163" height="52" rx="8" fill="var(--dga-primary-400)" />
                    
                    <rect x="216.5" y="1037.5" width="163" height="52" rx="8" fill="var(--dga-primary-400)" />
                    
                    <rect x="216.5" y="1101.5" width="163" height="52" rx="8" fill="var(--dga-primary-400)" />
                    
                    <rect x="216.5" y="1165.5" width="163" height="52" rx="8" fill="var(--dga-primary-400)" />
                    
                    <rect x="216.5" y="1229.5" width="163" height="52" rx="8" fill="var(--dga-primary-400)" />
                    
                    <rect x="216.5" y="845.5" width="163" height="52" rx="8" fill="var(--dga-primary-100)" />
                    
                    <rect x="216.5" y="909.5" width="163" height="52" rx="8" fill="var(--dga-primary-100)" />
                    
                    <rect x="216.5" y="973.5" width="163" height="52" rx="8" fill="var(--dga-primary-100)" />
                    
                    <rect x="216.5" y="717.5" width="163" height="52" rx="8" stroke="var(--dga-primary-400)" />
                    
                    <rect x="216.5" y="493.5" width="163" height="80" rx="8" fill="var(--dga-primary-700)" />
                    
                    <circle cx="946.5" cy="1229.5" r="3" transform="rotate(-90 946.5 1229.5)" fill="var(--dga-primary-200)" />
                    <rect x="309.5" y="1415.5" width="135" height="52.7573" rx="8" stroke="var(--dga-primary-200)" />
                    
                    <rect x="789.5" y="1415.5" width="135" height="52.7573" rx="8" stroke="var(--dga-primary-200)" />
                    
                    <rect x="949.5" y="1415.5" width="135" height="52.7573" rx="8" stroke="var(--dga-primary-200)" />
                    
                    <rect x="309.5" y="1493.62" width="135" height="52.7573" rx="8" stroke="var(--dga-primary-200)" />
                    
                    <rect x="789.5" y="1493.62" width="135" height="52.7573" rx="8" stroke="var(--dga-primary-200)" />
                    
                    <rect x="949.5" y="1493.62" width="135" height="52.7573" rx="8" stroke="var(--dga-primary-200)" />
                    
                    <rect x="149.5" y="1415.5" width="135" height="52.7573" rx="8" stroke="var(--dga-primary-200)" />
                    
                    <rect x="629.5" y="1415.5" width="135" height="52.7573" rx="8" stroke="var(--dga-primary-200)" />
                    
                    <rect x="149.5" y="1493.62" width="135" height="52.7573" rx="8" stroke="var(--dga-primary-200)" />
                    
                    <rect x="629.5" y="1493.62" width="135" height="52.7573" rx="8" stroke="var(--dga-primary-200)" />
                    
                    <rect x="949.5" y="1571.74" width="135" height="52.7573" rx="8" stroke="var(--dga-primary-200)" />
                    
                    <rect x="469.5" y="1415.5" width="135" height="52.7573" rx="8" stroke="var(--dga-primary-200)" />
                    
                    <rect x="469.5" y="1571.74" width="135" height="52.7573" rx="8" stroke="var(--dga-primary-200)" />
                    
                    <rect x="789.5" y="1571.74" width="135" height="52.7573" rx="8" stroke="var(--dga-primary-200)" />
                    
                    <rect x="309.5" y="1571.74" width="135" height="52.7573" rx="8" stroke="var(--dga-primary-200)" />
                    
                    <rect x="629.5" y="1571.74" width="135" height="52.7573" rx="8" stroke="var(--dga-primary-200)" />
                    
                    <rect x="149.5" y="1571.74" width="135" height="52.7573" rx="8" stroke="var(--dga-primary-200)" />
                    
                    <rect x="469.5" y="1493.62" width="135" height="52.7573" rx="8" stroke="var(--dga-primary-200)" />
                    
                    <rect x="125.5" y="1390.5" width="983" height="258" rx="24" stroke="var(--dga-primary-200)" stroke-width="2"
                        stroke-linecap="round" stroke-linejoin="round" />
                    <rect x="655.5" y="589.5" width="163" height="52" rx="8" fill="var(--dga-primary-100)" />
                    
                    <rect x="655.5" y="653.5" width="163" height="52" rx="8" fill="var(--dga-primary-100)" />
                    
                    <rect x="655.5" y="717.5" width="163" height="52" rx="8" fill="var(--dga-primary-100)" />
                    
                    <rect x="426.5" y="589.5" width="163" height="52" rx="8" fill="var(--dga-primary-200)" />
                    
                    <rect x="426.5" y="653.5" width="135" height="52" rx="8" fill="var(--dga-primary-400)" />
                    
                    <rect x="426.5" y="781.5" width="135" height="52" rx="8" fill="var(--dga-primary-100)" />
                    
                    <rect x="844.5" y="808.5" width="66" height="2" rx="1" transform="rotate(-90 844.5 808.5)"
                        fill="var(--dga-primary-300)" />
                    <rect x="858.5" y="744.5" width="14" height="2" rx="0.999999" transform="rotate(-180 858.5 744.5)"
                        fill="var(--dga-primary-300)" />
                    <circle cx="857.5" cy="743.5" r="3" transform="rotate(-180 857.5 743.5)" fill="var(--dga-primary-300)" />
                    <rect x="858.5" y="808.5" width="14" height="2" rx="0.999999" transform="rotate(-180 858.5 808.5)"
                        fill="var(--dga-primary-300)" />
                    <circle cx="857.5" cy="807.5" r="3" transform="rotate(-180 857.5 807.5)" fill="var(--dga-primary-300)" />
                    <rect x="769.5" y="197.5" width="75" height="2" rx="1" fill="var(--dga-primary-400)" />
                    <circle cx="843.5" cy="198.5" r="3" transform="rotate(90 843.5 198.5)" fill="var(--dga-primary-400)" />
                    <rect x="101.5" y="431.5" width="1027" height="2" rx="1" fill="var(--dga-primary-300)" />
                    <rect x="621.5" y="432.5" width="198" height="2" rx="1" transform="rotate(-90 621.5 432.5)"
                        fill="var(--dga-primary-300)" />
                    <rect x="621.5" y="1256.5" width="825" height="2" rx="1" transform="rotate(-90 621.5 1256.5)"
                        fill="var(--dga-primary-300)" />
                    <rect x="101.5" y="481.5" width="50" height="2" rx="1" transform="rotate(-90 101.5 481.5)"
                        fill="var(--dga-primary-300)" />
                    <circle cx="102.5" cy="482.5" r="3" transform="rotate(-90 102.5 482.5)" fill="var(--dga-primary-300)" />
                    <rect x="931.5" y="481.5" width="50" height="2" rx="1" transform="rotate(-90 931.5 481.5)"
                        fill="var(--dga-primary-300)" />
                    <circle cx="932.5" cy="482.5" r="3" transform="rotate(-90 932.5 482.5)" fill="var(--dga-primary-300)" />
                    <rect x="300.5" y="481.5" width="50" height="2" rx="1" transform="rotate(-90 300.5 481.5)"
                        fill="var(--dga-primary-300)" />
                    <circle cx="301.5" cy="482.5" r="3" transform="rotate(-90 301.5 482.5)" fill="var(--dga-primary-300)" />
                    <rect x="1126.5" y="481.5" width="50" height="2" rx="1" transform="rotate(-90 1126.5 481.5)"
                        fill="var(--dga-primary-300)" />
                    <circle cx="1127.5" cy="482.5" r="3" transform="rotate(-90 1127.5 482.5)" fill="var(--dga-primary-300)" />
                    <rect x="640.5" y="616.5" width="19" height="2" rx="0.999999" transform="rotate(-180 640.5 616.5)"
                        fill="var(--dga-primary-300)" />
                    <circle cx="639.5" cy="615.5" r="3" transform="rotate(-180 639.5 615.5)" fill="var(--dga-primary-300)" />
                    <rect x="604.5" y="614.5" width="19" height="2" rx="0.999999" fill="var(--dga-primary-300)" />
                    <circle cx="604.5" cy="615.5" r="3" fill="var(--dga-primary-300)" />
                    <rect x="640.5" y="680.5" width="19" height="2" rx="0.999999" transform="rotate(-180 640.5 680.5)"
                        fill="var(--dga-primary-300)" />
                    <circle cx="639.5" cy="679.5" r="3" transform="rotate(-180 639.5 679.5)" fill="var(--dga-primary-300)" />
                    <rect x="640.5" y="744.5" width="19" height="2" rx="0.999999" transform="rotate(-180 640.5 744.5)"
                        fill="var(--dga-primary-300)" />
                    <circle cx="639.5" cy="743.5" r="3" transform="rotate(-180 639.5 743.5)" fill="var(--dga-primary-300)" />
                    <rect x="640.5" y="808.5" width="19" height="2" rx="0.999999" transform="rotate(-180 640.5 808.5)"
                        fill="var(--dga-primary-300)" />
                    <circle cx="639.5" cy="807.5" r="3" transform="rotate(-180 639.5 807.5)" fill="var(--dga-primary-300)" />
                    <rect x="640.5" y="872.5" width="19" height="2" rx="0.999999" transform="rotate(-180 640.5 872.5)"
                        fill="var(--dga-primary-300)" />
                    <circle cx="639.5" cy="871.5" r="3" transform="rotate(-180 639.5 871.5)" fill="var(--dga-primary-300)" />
                    <rect x="604.5" y="870.5" width="19" height="2" rx="0.999999" fill="var(--dga-primary-300)" />
                    <circle cx="604.5" cy="871.5" r="3" fill="var(--dga-primary-300)" />
                    <rect x="640.5" y="1256.5" width="19" height="2" rx="0.999999" transform="rotate(-180 640.5 1256.5)"
                        fill="var(--dga-primary-300)" />
                    <circle cx="639.5" cy="1255.5" r="3" transform="rotate(-180 639.5 1255.5)" fill="var(--dga-primary-300)" />
                    <rect x="640.5" y="936.5" width="19" height="2" rx="0.999999" transform="rotate(-180 640.5 936.5)"
                        fill="var(--dga-primary-300)" />
                    <circle cx="639.5" cy="935.5" r="3" transform="rotate(-180 639.5 935.5)" fill="var(--dga-primary-300)" />
                    <rect x="604.5" y="934.5" width="19" height="2" rx="0.999999" fill="var(--dga-primary-300)" />
                    <circle cx="604.5" cy="935.5" r="3" fill="var(--dga-primary-300)" />
                    <rect x="640.5" y="1000.5" width="19" height="2" rx="0.999999" transform="rotate(-180 640.5 1000.5)"
                        fill="var(--dga-primary-300)" />
                    <circle cx="639.5" cy="999.5" r="3" transform="rotate(-180 639.5 999.5)" fill="var(--dga-primary-300)" />
                    <rect x="604.5" y="998.5" width="19" height="2" rx="0.999999" fill="var(--dga-primary-300)" />
                    <circle cx="604.5" cy="999.5" r="3" fill="var(--dga-primary-300)" />
                    <rect x="640.5" y="1064.5" width="19" height="2" rx="0.999999" transform="rotate(-180 640.5 1064.5)"
                        fill="var(--dga-primary-300)" />
                    <circle cx="639.5" cy="1063.5" r="3" transform="rotate(-180 639.5 1063.5)" fill="var(--dga-primary-300)" />
                    <rect x="604.5" y="1062.5" width="19" height="2" rx="0.999999" fill="var(--dga-primary-300)" />
                    <circle cx="604.5" cy="1063.5" r="3" fill="var(--dga-primary-300)" />
                    <rect x="640.5" y="1128.5" width="19" height="2" rx="0.999999" transform="rotate(-180 640.5 1128.5)"
                        fill="var(--dga-primary-300)" />
                    <circle cx="639.5" cy="1127.5" r="3" transform="rotate(-180 639.5 1127.5)" fill="var(--dga-primary-300)" />
                    <rect x="604.5" y="1126.5" width="19" height="2" rx="0.999999" fill="var(--dga-primary-300)" />
                    <circle cx="604.5" cy="1127.5" r="3" fill="var(--dga-primary-300)" />
                    <rect x="640.5" y="1192.5" width="19" height="2" rx="0.999999" transform="rotate(-180 640.5 1192.5)"
                        fill="var(--dga-primary-300)" />
                    <circle cx="639.5" cy="1191.5" r="3" transform="rotate(-180 639.5 1191.5)" fill="var(--dga-primary-300)" />
                    <rect x="604.5" y="1190.5" width="19" height="2" rx="0.999999" fill="var(--dga-primary-300)" />
                    <circle cx="604.5" cy="1191.5" r="3" fill="var(--dga-primary-300)" />
                    <rect x="575.5" y="807.5" width="154" height="2" rx="1" transform="rotate(-90 575.5 807.5)"
                        fill="var(--dga-primary-300)" />
                    <circle cx="576.5" cy="679.5" r="3" transform="rotate(-90 576.5 679.5)" fill="var(--dga-primary-300)" />
                    <circle cx="576.5" cy="743.5" r="3" transform="rotate(-90 576.5 743.5)" fill="var(--dga-primary-300)" />
                    <circle cx="576.5" cy="807.5" r="3" transform="rotate(-90 576.5 807.5)" fill="var(--dga-primary-300)" />
                    <rect x="426.5" y="717.5" width="135" height="52" rx="8" fill="var(--dga-primary-100)" />
                    
                    <rect x="426.5" y="1037.5" width="163" height="52" rx="8" fill="var(--dga-primary-200)" />
                    
                    <rect x="426.5" y="973.5" width="163" height="52" rx="8" fill="var(--dga-primary-200)" />
                    
                    <rect x="426.5" y="909.5" width="163" height="52" rx="8" fill="var(--dga-primary-200)" />
                    
                    <rect x="426.5" y="1101.5" width="163" height="52" rx="8" fill="var(--dga-primary-200)" />
                    
                    <rect x="426.5" y="845.5" width="163" height="52" rx="8" fill="var(--dga-primary-200)" />
                    
                    <rect x="426.5" y="1165.5" width="163" height="52" rx="8" fill="var(--dga-primary-200)" />
                    
                    <rect x="154.875" y="12.375" width="23.25" height="15.75" rx="1.875" stroke="var(--dga-primary-400)"
                        stroke-width="0.75" />
                    
                    <rect x="154.875" y="40.125" width="23.25" height="15.75" rx="1.875" fill="var(--dga-primary-700)" stroke="var(--dga-primary-700)"
                        stroke-width="0.75" />
                    
                    <rect x="154.875" y="67.875" width="23.25" height="15.75" rx="1.875" fill="var(--dga-primary-200)" stroke="var(--dga-primary-200)"
                        stroke-width="0.75" />
                    
                    <rect x="154.875" y="95.625" width="23.25" height="15.75" rx="1.875" stroke="var(--dga-primary-200)"
                        stroke-width="0.75" />
                    
                    
                    <rect x="154.875" y="123.375" width="23.25" height="15.75" rx="1.875" fill="var(--dga-primary-500)"
                        stroke="var(--dga-primary-500)" stroke-width="0.75" />
                    
                    <rect x="154.875" y="151.125" width="23.25" height="15.75" rx="1.875" fill="var(--dga-primary-100)"
                        stroke="var(--dga-primary-100)" stroke-width="0.75" />
                    
                    <rect x="154.875" y="178.875" width="23.25" height="15.75" rx="1.875" fill="var(--dga-primary-50)"
                        stroke="var(--dga-primary-50)" stroke-width="0.75" />
                    
                    <rect x="154.875" y="206.625" width="23.25" height="15.75" rx="1.875" fill="var(--dga-primary-400)"
                        stroke="var(--dga-primary-400)" stroke-width="0.75" />
                </svg>
            <div class="org-overlay">
                <asp:Repeater ID="rptLegend" runat="server">
                    <ItemTemplate>
                        <div class="org-legend-label" style="<%# Eval("Style") %>"><%# Eval("Label") %></div>
                    </ItemTemplate>
                </asp:Repeater>
                <asp:Repeater ID="rptLabels" runat="server">
                    <ItemTemplate>
                        <div class="org-box-label <%# Eval("LinkClass") %>" style="<%# Eval("Style") %>"
                            data-title="<%# Eval("Title") %>" data-desc="<%# Eval("Desc") %>"
                            data-icon="<%# Eval("Icon") %>" data-href="<%# Eval("Href") %>"
                            data-has-link="<%# Eval("HasLink") %>" tabindex="0" role="group"
                            aria-label="<%# Eval("AriaLabel") %>">
                            <i class="obl-icon <%# Eval("IconClass") %>" aria-hidden="true"></i>
                            <span class="obl-text"><%# Eval("Label") %></span>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>

            <div class="org-unit-tooltip" id="orgUnitTooltip" role="group"
                aria-labelledby="orgUnitTooltipTitle" aria-hidden="true" hidden>
                <div class="card nav-card h-100">
                    <div class="d-flex card-body flex-column gap-4">
                        <div class="icon-container">
                            <i class="hgi hgi-stroke hgi-hierarchy fs-3" id="orgUnitTooltipIcon" aria-hidden="true"></i>
                        </div>
                        <div>
                            <h3 class="card-title" id="orgUnitTooltipTitle"></h3>
                            <p class="card-text" id="orgUnitTooltipText"></p>
                        </div>
                        <div class="d-flex justify-content-end mt-auto">
                            <a class="btn btn-secondary stretched-link" id="orgUnitTooltipLink" aria-label="" hidden>
                                <i class="hgi hgi-stroke hgi-arrow-left-02 fs-4" aria-hidden="true"></i>
                            </a>
                            <button class="btn btn-secondary stretched-link" id="orgUnitTooltipDisabledAction"
                                type="button" aria-label="" disabled hidden>
                                <i class="hgi hgi-stroke hgi-arrow-left-02 fs-4" aria-hidden="true"></i>
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>

        <asp:Literal ID="litConfig" runat="server" EnableViewState="false" />
    </div>
</section>

<script>
    (function () {
        var cfg = window.__pnuOrgConfig || { fallbackTitle: "", noDetails: "" };

        function initOrgStructure() {
            var tooltip = document.getElementById("orgUnitTooltip");
            var tTitle = document.getElementById("orgUnitTooltipTitle");
            var tText = document.getElementById("orgUnitTooltipText");
            var tIcon = document.getElementById("orgUnitTooltipIcon");
            var tLink = document.getElementById("orgUnitTooltipLink");
            var tDisabled = document.getElementById("orgUnitTooltipDisabledAction");
            if (!tooltip) { return; }

            var labels = document.querySelectorAll(".org-box-label");
            var finePointer = window.matchMedia("(hover: hover) and (pointer: fine)");
            var activeEl = null, hideTimer = null, frameId = null, lastPoint = { x: 0, y: 0 };

            function populate(el) {
                var d = el.dataset;
                tTitle.textContent = d.title || cfg.fallbackTitle;
                tText.textContent = d.desc || "";
                tIcon.className = "hgi hgi-stroke " + (d.icon || "hgi-hierarchy") + " fs-3";
                if (d.href) {
                    tLink.href = d.href;
                    tLink.setAttribute("aria-label", d.title || cfg.fallbackTitle);
                    tLink.hidden = false; tDisabled.hidden = true;
                } else {
                    tLink.removeAttribute("href"); tLink.hidden = true;
                    tDisabled.hidden = false;
                    tDisabled.setAttribute("aria-label", (d.title || cfg.fallbackTitle) + "، " + cfg.noDetails);
                }
            }
            function queue(x, y) {
                lastPoint = { x: x, y: y };
                if (frameId) { return; }
                frameId = window.requestAnimationFrame(function () {
                    frameId = null;
                    if (tooltip.hidden) { return; }
                    var s = 20, b = tooltip.getBoundingClientRect();
                    var left = lastPoint.x + s, top = lastPoint.y - (b.height / 2);
                    if (left + b.width > window.innerWidth - 16) { left = lastPoint.x - b.width - s; }
                    if (left < 16) { left = 16; }
                    if (top + b.height > window.innerHeight - 16) { top = window.innerHeight - b.height - 16; }
                    if (top < 16) { top = 16; }
                    tooltip.style.left = Math.round(left) + "px";
                    tooltip.style.top = Math.round(top) + "px";
                });
            }
            function cancelHide() { if (hideTimer) { window.clearTimeout(hideTimer); hideTimer = null; } }
            function anchor(el) { var r = el.getBoundingClientRect(); return { x: r.right - 12, y: r.top + r.height / 2 }; }
            function show(el) {
                cancelHide();
                if (activeEl && activeEl !== el) { activeEl.classList.remove("is-active"); }
                activeEl = el; el.classList.add("is-active");
                populate(el);
                tooltip.hidden = false; tooltip.setAttribute("aria-hidden", "false");
                var p = anchor(el); queue(p.x, p.y);
                window.requestAnimationFrame(function () { tooltip.classList.add("is-visible"); });
            }
            function hide() {
                if (activeEl) { activeEl.classList.remove("is-active"); }
                activeEl = null; tooltip.classList.remove("is-visible");
                tooltip.hidden = true; tooltip.setAttribute("aria-hidden", "true");
            }
            function scheduleHide() { cancelHide(); hideTimer = window.setTimeout(function () { hideTimer = null; hide(); }, 240); }
            function navigate(el) { var h = el.dataset.href; if (h) { window.location.href = h; } }

            Array.prototype.forEach.call(labels, function (el) {
                el.addEventListener("pointerenter", function (e) { if (finePointer.matches && e.pointerType === "mouse") { show(el); } });
                el.addEventListener("pointerleave", function (e) { if (e.pointerType === "mouse") { scheduleHide(); } });
                el.addEventListener("focus", function () { show(el); });
                el.addEventListener("blur", function () { scheduleHide(); });
                el.addEventListener("click", function () { navigate(el); });
                el.addEventListener("keydown", function (e) {
                    if (e.key === "Enter" || e.key === " ") { e.preventDefault(); navigate(el); }
                });
            });

            tooltip.addEventListener("pointerenter", function (e) { if (e.pointerType === "mouse") { cancelHide(); } });
            tooltip.addEventListener("pointerleave", function (e) { if (e.pointerType === "mouse") { scheduleHide(); } });
            window.addEventListener("scroll", function () {
                if (!activeEl || tooltip.hidden) { return; }
                var p = anchor(activeEl); queue(p.x, p.y);
            }, { passive: true });
            window.addEventListener("resize", function () { hide(); });
            document.addEventListener("keydown", function (e) { if (e.key === "Escape") { hide(); } });
        }

        window.addEventListener("load", initOrgStructure);
    })();
</script>
