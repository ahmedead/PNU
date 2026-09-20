<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserControl2.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Forms.UserControl2" %>






<div class="-a-24">
    <div aria-labelledby="FormTitleId_titleAriaId" class="css-28" role="main">
        <div class="-a-29">
            <div class="__title__ -P-30 " aria-labelledby="FormTitleId_titleAriaId" tabindex="-1">
                <div class="-lK-32">
                    <div class="-r-37">
                        
                        <div class="-a-242">
                            <div class="css-247">
                                <button aria-label="More options" aria-expanded="false" role="button" aria-haspopup="true" tabindex="0" aria-live="polite" aria-controls="ImmersiveReaderMenu" class="css-251"><span class="-a-252">
                                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" class="---253">
                                        <path d="M7.75 12a1.75 1.75 0 1 1-3.5 0 1.75 1.75 0 0 1 3.5 0Zm6 0a1.75 1.75 0 1 1-3.5 0 1.75 1.75 0 0 1 3.5 0ZM18 13.75a1.75 1.75 0 1 0 0-3.5 1.75 1.75 0 0 0 0 3.5Z"></path></svg></span></button></div>
                        </div>
                    </div>
                    <div class="-pb-31">
                        <div class="-nb-41"></div>
                        <div id="FormTitleId_titleAriaId" class="-eJ-33">
                            <div aria-level="1" role="heading" class="-m-35" data-automation-id="formTitle"><span class="text-format-content ">
                                <span style="color: rgb(0, 0, 0); font-size: 0.9em;">طلب بيانات مفتوحة<br>
                            </span>
                                <br>
                            </span></div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="css-1" role="status" id="FormTitleId_EnableScreenReader">Immersive Reader in Microsoft Forms allows you to hear the text of a form title and questions read out loud while following along. You can find the Immersive Reader button next to form title or questions after activating this control. You can also change the spacing of line and words to make them easier to read, highlight parts of speech and syllables, select single words or lines of words read aloud, and select language preferences.</div>
        </div>
        <div class="css-42">
            <div data-automation-id="noticeContainer" class="css-45 ">
                <div><span class="-sR-44" aria-hidden="true"></span><span>Required</span></div>
            </div>
            <div id="question-list">
                <div data-automation-id="questionItem" class="css-50 ">
                    <div id="QuestionId_rae38396d43a94d7ba57b78e029af430d" class="-a-53">
                        <div class="-P-54"><span class="-m-56" data-automation-id="questionTitle"><span aria-level="2" role="heading"><span data-automation-id="questionOrdinal" aria-hidden="false" class="-ht-58">1.</span><span class="text-format-content ">الاسم</span><span aria-hidden="false" class="css-4" role="note" aria-label="Required to answer" data-automation-id="requiredStar"></span></span><span class="-ac-57"><span class="css-1" id="QuestionInfo_rae38396d43a94d7ba57b78e029af430d" aria-hidden="true">Single line text. </span></span><span class="-fn-59" role="log" aria-live="polite"></span></span></div>
                        <div class="-kJ-55"></div>
                    </div>
                    <div class="-bT-49">
                        <div class="-a-61 lrp-text-container"><span class="-nd-68">
                            <input aria-label="Single line text" maxlength="4000" placeholder="Enter your answer" aria-labelledby="QuestionId_rae38396d43a94d7ba57b78e029af430d QuestionInfo_rae38396d43a94d7ba57b78e029af430d" class="-as-67" spellcheck="false" data-automation-id="textInput" data-ms-editor="true"></span></div>
                    </div>
                </div>
                <div data-automation-id="questionItem" class="css-50 ">
                    <div id="QuestionId_r31ec9e680f434dbd855f143f1d593bca" class="-a-53">
                        <div class="-P-54"><span class="-m-56" data-automation-id="questionTitle"><span aria-level="2" role="heading"><span data-automation-id="questionOrdinal" aria-hidden="false" class="-ht-58">2.</span><span class="text-format-content ">البريد الإلكتروني<br>
                        </span><span aria-hidden="false" class="css-4" role="note" aria-label="Required to answer" data-automation-id="requiredStar"></span></span><span class="-ac-57"><span class="css-1" id="QuestionInfo_r31ec9e680f434dbd855f143f1d593bca" aria-hidden="true">Single line text. </span></span><span class="-fn-59" role="log" aria-live="polite"></span></span></div>
                        <div class="-kJ-55"></div>
                    </div>
                    <div class="-bT-49">
                        <div class="-a-61 lrp-text-container"><span class="-nd-68">
                            <input aria-label="Single line text" maxlength="4000" placeholder="Enter your answer" aria-labelledby="QuestionId_r31ec9e680f434dbd855f143f1d593bca QuestionInfo_r31ec9e680f434dbd855f143f1d593bca" class="-as-67" spellcheck="false" data-automation-id="textInput" data-ms-editor="true"></span></div>
                    </div>
                </div>
                <div data-automation-id="questionItem" class="css-50 ">
                    <div id="QuestionId_r62c03ada4fe44d4a8a03af8ed7d39691" class="-a-53">
                        <div class="-P-54"><span class="-m-56" data-automation-id="questionTitle"><span aria-level="2" role="heading"><span data-automation-id="questionOrdinal" aria-hidden="false" class="-ht-58">3.</span><span class="text-format-content "><span>شريحة المستفيدين من البيانات المفتوحة</span></span><span aria-hidden="false" class="css-4" role="note" aria-label="Required to answer" data-automation-id="requiredStar"></span></span><span class="-ac-57"><span class="css-1" id="QuestionInfo_r62c03ada4fe44d4a8a03af8ed7d39691" aria-hidden="true">Multiple choice. </span></span><span class="-fn-59" role="log" aria-live="polite"></span></span></div>
                        <div class="-kJ-55"></div>
                    </div>
                    <div class="-bT-49">
                        <div role="group" aria-labelledby="QuestionId_r62c03ada4fe44d4a8a03af8ed7d39691 QuestionInfo_r62c03ada4fe44d4a8a03af8ed7d39691" class="-a-73">
                            <div class="-a-82">
                                <div data-automation-id="choiceItem" class="">
                                    <div>
                                        <label class="--q-84"><span class="-a-85" data-automation-id="checkbox" data-automation-value="باحث">
                                            <input name="r62c03ada4fe44d4a8a03af8ed7d39691" aria-labelledby="QuestionChoiceOption1" aria-checked="false" role="checkbox" type="checkbox" class="-h_-86" value="باحث"><span class="-a-87"><svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" class="---88"><rect width="19" height="19" x="2.5" y="2.5" fill="#fff" rx="1.5"></rect></svg></span></span><span aria-label="باحث" id="QuestionChoiceOption1" class="text-format-content css-89">باحث</span></label></div>
                                </div>
                                <div data-automation-id="choiceItem" class="">
                                    <div>
                                        <label class="--q-84"><span class="-a-85" data-automation-id="checkbox" data-automation-value="طالب">
                                            <input name="r62c03ada4fe44d4a8a03af8ed7d39691" aria-labelledby="QuestionChoiceOption2" aria-checked="false" role="checkbox" type="checkbox" class="-h_-86" value="طالب"><span class="-a-87"><svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" class="---88"><rect width="19" height="19" x="2.5" y="2.5" fill="#fff" rx="1.5"></rect></svg></span></span><span aria-label="طالب" id="QuestionChoiceOption2" class="text-format-content css-89">طالب</span></label></div>
                                </div>
                                <div data-automation-id="choiceItem" class="">
                                    <div>
                                        <label class="--q-84"><span class="-a-85" data-automation-id="checkbox" data-automation-value="موظف">
                                            <input name="r62c03ada4fe44d4a8a03af8ed7d39691" aria-labelledby="QuestionChoiceOption3" aria-checked="false" role="checkbox" type="checkbox" class="-h_-86" value="موظف"><span class="-a-87"><svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" class="---88"><rect width="19" height="19" x="2.5" y="2.5" fill="#fff" rx="1.5"></rect></svg></span></span><span aria-label="موظف" id="QuestionChoiceOption3" class="text-format-content css-89">موظف</span></label></div>
                                </div>
                            </div>
                            <div class="-mR-75">
                                <label class="-me-74"><span class="-a-85" data-automation-id="checkbox">
                                    <input aria-label="Other answer" name="r62c03ada4fe44d4a8a03af8ed7d39691" aria-checked="false" role="checkbox" type="checkbox" class="-h_-86" value=""><span class="-a-87"><svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" class="---88"><rect width="19" height="19" x="2.5" y="2.5" fill="#fff" rx="1.5"></rect></svg></span></span><div class="-jI-76"><span class="-nd-91">
                                        <input aria-label="Other answer" placeholder="Other" maxlength="1000" class="-as-67" spellcheck="false" data-automation-id="textInput" data-ms-editor="true"></span></div>
                                </label>
                            </div>
                        </div>
                    </div>
                </div>
                <div data-automation-id="questionItem" class="css-50 ">
                    <div id="QuestionId_r560dd526413f4f83a4aff500ccc393ac" class="-a-53">
                        <div class="-P-54"><span class="-m-56" data-automation-id="questionTitle"><span aria-level="2" role="heading"><span data-automation-id="questionOrdinal" aria-hidden="false" class="-ht-58">4.</span><span class="text-format-content ">مجموعة البيانات المطلوبة<br>
                        </span></span><span class="-ac-57"><span class="css-1" id="QuestionInfo_r560dd526413f4f83a4aff500ccc393ac" aria-hidden="true">Multiple choice. </span></span><span class="-fn-59" role="log" aria-live="polite"></span></span></div>
                        <div class="-kJ-55"></div>
                    </div>
                    <div class="-bT-49">
                        <div role="group" aria-labelledby="QuestionId_r560dd526413f4f83a4aff500ccc393ac QuestionInfo_r560dd526413f4f83a4aff500ccc393ac" class="-a-73">
                            <div class="-a-82">
                                <div data-automation-id="choiceItem" class="">
                                    <div>
                                        <label class="--q-84"><span class="-a-85" data-automation-id="checkbox" data-automation-value="الجامعة في أرقام">
                                            <input name="r560dd526413f4f83a4aff500ccc393ac" aria-labelledby="QuestionChoiceOption4" aria-checked="false" role="checkbox" type="checkbox" class="-h_-86" value="الجامعة في أرقام"><span class="-a-87"><svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" class="---88"><rect width="19" height="19" x="2.5" y="2.5" fill="#fff" rx="1.5"></rect></svg></span></span><span aria-label="الجامعة في أرقام" id="QuestionChoiceOption4" class="text-format-content css-89">الجامعة في أرقام</span></label></div>
                                </div>
                                <div data-automation-id="choiceItem" class="">
                                    <div>
                                        <label class="--q-84"><span class="-a-85" data-automation-id="checkbox" data-automation-value="الموظفين">
                                            <input name="r560dd526413f4f83a4aff500ccc393ac" aria-labelledby="QuestionChoiceOption5" aria-checked="false" role="checkbox" type="checkbox" class="-h_-86" value="الموظفين"><span class="-a-87"><svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" class="---88"><rect width="19" height="19" x="2.5" y="2.5" fill="#fff" rx="1.5"></rect></svg></span></span><span aria-label="الموظفين" id="QuestionChoiceOption5" class="text-format-content css-89">الموظفين</span></label></div>
                                </div>
                                <div data-automation-id="choiceItem" class="">
                                    <div>
                                        <label class="--q-84"><span class="-a-85" data-automation-id="checkbox" data-automation-value="الطالبات">
                                            <input name="r560dd526413f4f83a4aff500ccc393ac" aria-labelledby="QuestionChoiceOption6" aria-checked="false" role="checkbox" type="checkbox" class="-h_-86" value="الطالبات"><span class="-a-87"><svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" class="---88"><rect width="19" height="19" x="2.5" y="2.5" fill="#fff" rx="1.5"></rect></svg></span></span><span aria-label="الطالبات" id="QuestionChoiceOption6" class="text-format-content css-89">الطالبات</span></label></div>
                                </div>
                                <div data-automation-id="choiceItem" class="">
                                    <div>
                                        <label class="--q-84"><span class="-a-85" data-automation-id="checkbox" data-automation-value="الأبحاث">
                                            <input name="r560dd526413f4f83a4aff500ccc393ac" aria-labelledby="QuestionChoiceOption7" aria-checked="false" role="checkbox" type="checkbox" class="-h_-86" value="الأبحاث"><span class="-a-87"><svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" class="---88"><rect width="19" height="19" x="2.5" y="2.5" fill="#fff" rx="1.5"></rect></svg></span></span><span aria-label="الأبحاث" id="QuestionChoiceOption7" class="text-format-content css-89">الأبحاث</span></label></div>
                                </div>
                                <div data-automation-id="choiceItem" class="">
                                    <div>
                                        <label class="--q-84"><span class="-a-85" data-automation-id="checkbox" data-automation-value="الخريجات">
                                            <input name="r560dd526413f4f83a4aff500ccc393ac" aria-labelledby="QuestionChoiceOption8" aria-checked="false" role="checkbox" type="checkbox" class="-h_-86" value="الخريجات"><span class="-a-87"><svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" class="---88"><rect width="19" height="19" x="2.5" y="2.5" fill="#fff" rx="1.5"></rect></svg></span></span><span aria-label="الخريجات" id="QuestionChoiceOption8" class="text-format-content css-89">الخريجات</span></label></div>
                                </div>
                                <div data-automation-id="choiceItem" class="">
                                    <div>
                                        <label class="--q-84"><span class="-a-85" data-automation-id="checkbox" data-automation-value="الخريجات في سوق العمل">
                                            <input name="r560dd526413f4f83a4aff500ccc393ac" aria-labelledby="QuestionChoiceOption9" aria-checked="false" role="checkbox" type="checkbox" class="-h_-86" value="الخريجات في سوق العمل"><span class="-a-87"><svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" class="---88"><rect width="19" height="19" x="2.5" y="2.5" fill="#fff" rx="1.5"></rect></svg></span></span><span aria-label="الخريجات في سوق العمل" id="QuestionChoiceOption9" class="text-format-content css-89">الخريجات في سوق العمل</span></label></div>
                                </div>
                                <div data-automation-id="choiceItem" class="">
                                    <div>
                                        <label class="--q-84"><span class="-a-85" data-automation-id="checkbox" data-automation-value="براءات الاختراع">
                                            <input name="r560dd526413f4f83a4aff500ccc393ac" aria-labelledby="QuestionChoiceOption10" aria-checked="false" role="checkbox" type="checkbox" class="-h_-86" value="براءات الاختراع"><span class="-a-87"><svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" class="---88"><rect width="19" height="19" x="2.5" y="2.5" fill="#fff" rx="1.5"></rect></svg></span></span><span aria-label="براءات الاختراع" id="QuestionChoiceOption10" class="text-format-content css-89">براءات الاختراع</span></label></div>
                                </div>
                                <div data-automation-id="choiceItem" class="">
                                    <div>
                                        <label class="--q-84"><span class="-a-85" data-automation-id="checkbox" data-automation-value="الجوائز">
                                            <input name="r560dd526413f4f83a4aff500ccc393ac" aria-labelledby="QuestionChoiceOption11" aria-checked="false" role="checkbox" type="checkbox" class="-h_-86" value="الجوائز"><span class="-a-87"><svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" class="---88"><rect width="19" height="19" x="2.5" y="2.5" fill="#fff" rx="1.5"></rect></svg></span></span><span aria-label="الجوائز" id="QuestionChoiceOption11" class="text-format-content css-89">الجوائز</span></label></div>
                                </div>
                                <div data-automation-id="choiceItem" class="">
                                    <div>
                                        <label class="--q-84"><span class="-a-85" data-automation-id="checkbox" data-automation-value="الشؤون الرياضية">
                                            <input name="r560dd526413f4f83a4aff500ccc393ac" aria-labelledby="QuestionChoiceOption12" aria-checked="false" role="checkbox" type="checkbox" class="-h_-86" value="الشؤون الرياضية"><span class="-a-87"><svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" class="---88"><rect width="19" height="19" x="2.5" y="2.5" fill="#fff" rx="1.5"></rect></svg></span></span><span aria-label="الشؤون الرياضية" id="QuestionChoiceOption12" class="text-format-content css-89">الشؤون الرياضية</span></label></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div data-automation-id="questionItem" class="css-50 ">
                    <div id="QuestionId_rec7f46fe0d684941a78001d710461a7e" class="-a-53">
                        <div class="-P-54"><span class="-m-56" data-automation-id="questionTitle"><span aria-level="2" role="heading"><span data-automation-id="questionOrdinal" aria-hidden="false" class="-ht-58">5.</span><span class="text-format-content ">طلب بيانات مفتوحة إضافية<br>
                        </span></span><span class="-ac-57"><span class="css-1" id="QuestionInfo_rec7f46fe0d684941a78001d710461a7e" aria-hidden="true">Multi Line Text. </span></span><span class="-fn-59" role="log" aria-live="polite"></span></span></div>
                        <div class="-kJ-55"></div>
                    </div>
                    <div class="-bT-49">
                        <div class="-a-61 lrp-text-container"><span class="-nd-68" style="">
                            <textarea aria-label="Multi Line Text" maxlength="4000" placeholder="Enter your answer" aria-labelledby="QuestionId_rec7f46fe0d684941a78001d710461a7e QuestionInfo_rec7f46fe0d684941a78001d710461a7e" class="-as-67" spellcheck="false" data-automation-id="textInput" style="height: 80px; min-height: 80px;" data-ms-editor="true"></textarea></span></div>
                    </div>
                </div>
                <div data-automation-id="questionItem" class="css-50 ">
                    <div id="QuestionId_r9a14187b8c35425087498030b0f900d0" class="-a-53">
                        <div class="-P-54"><span class="-m-56" data-automation-id="questionTitle"><span aria-level="2" role="heading"><span data-automation-id="questionOrdinal" aria-hidden="false" class="-ht-58">6.</span><span class="text-format-content ">مقترحات<br>
                        </span></span><span class="-ac-57"><span class="css-1" id="QuestionInfo_r9a14187b8c35425087498030b0f900d0" aria-hidden="true">Multi Line Text. </span></span><span class="-fn-59" role="log" aria-live="polite"></span></span></div>
                        <div class="-kJ-55"></div>
                    </div>
                    <div class="-bT-49">
                        <div class="-a-61 lrp-text-container"><span class="-nd-68" style="">
                            <textarea aria-label="Multi Line Text" maxlength="4000" placeholder="Enter your answer" aria-labelledby="QuestionId_r9a14187b8c35425087498030b0f900d0 QuestionInfo_r9a14187b8c35425087498030b0f900d0" class="-as-67" spellcheck="false" data-automation-id="textInput" style="height: 80px; min-height: 80px;" data-ms-editor="true"></textarea></span></div>
                    </div>
                </div>
            </div>
            <div class="css-220">
                <div class="-a-221">
                    <button data-automation-id="submitButton" class="css-222">Submit</button></div>
            </div>
            <div class="-a-229">Never give out your password.<a class="css-231" tabindex="0" role="link">Report abuse</a></div>
        </div>
        <div id="branding-footer">
            <div class="-dv-106">
                <img src="https://cdn.forms.office.net/images/microsoft365logo_v1.png" alt="Microsoft 365" tabindex="0" height="60" aria-label="Microsoft 365" role="link"></div>
            <div class="-r-102" role="contentinfo">
                <div class="-a-103">
                    <div class="-cH-104">This content is created by the owner of the form. The data you submit will be sent to the form owner. Microsoft is not responsible for the privacy or security practices of its customers, including those of this form owner. Never give out your password.</div>
                    <div class="-cH-104">
                        <div>
                            <button class="-da-111" tabindex="0" role="link">Microsoft Forms</button><span> | </span><span>AI-Powered surveys, quizzes and polls</span><span class="-oO-108"><a class="css-112" tabindex="0" role="link">Create my own form</a></span></div>
                        <div class="-rs-109"><span>The owner of this form has not provided a privacy statement as to how they will use your response data. Do not provide personal or sensitive information.</span><span role="presentation"> | </span><a rel="noreferrer" target="_blank" class="-gB-110 css-112" tabindex="0" role="link" href="https://go.microsoft.com/fwlink/?linkid=866263">Terms of use</a></div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>