

console.log("attachmentUpload.js loaded successfully.");

//document.addEventListener("DOMContentLoaded", function () {
//    // Attach listener for dynamically loaded components
//    const attachmentContainer = document.querySelector("#attachmentComponentContainer");
//    const statusButtonsContainer = document.getElementById('statusButtons');
//    if (statusButtonsContainer) {
//        statusButtonsContainer.addEventListener('click', function (event) {
//            if (event.target.matches('.submitAttachmentButton')) {
//                moveToNextStatus(event.target);
//            }
//        });
//    }
//    if (statusButtonsContainer) {
//        statusButtonsContainer.addEventListener('click', function (event) {
//            if (event.target.matches('.submitAttachmentButton')) {
//                moveToNextStatus(event.target);
//            }
//        });
//    }
//    if (attachmentContainer) {
//        attachmentContainer.addEventListener("DOMNodeInserted", function () {
//            initializeAttachmentUpload();
//        });
//    }

//    // Call immediately for direct rendering
//    if (typeof initializeAttachmentUpload === "function") {
//        initializeAttachmentUpload();
//    }
//});
//document.addEventListener("DOMContentLoaded", function () {
//    console.log("attachmentUpload.js loaded successfully.");

//    // Attach listener for dynamically loaded components
//    const attachmentContainer = document.querySelector("#attachmentComponentContainer");
//    const statusButtonsContainer = document.getElementById('statusButtons');

//    // Ensure that initializeAttachmentUpload is called when the page is loaded
//    initializeAttachmentUpload();

//    // Attach event listener for the submit button(s)
//    const submitButtons = document.querySelectorAll(".submitAttachmentButton");

//    submitButtons.forEach(button => {
//        button.addEventListener("click", function () {
//            initializeAttachmentUpload(); // Perform the action for each button
//        });
//    });

//    // Add event listener to status buttons
//    if (statusButtonsContainer) {
//        statusButtonsContainer.addEventListener('click', function (event) {
//            if (event.target.matches('.submitAttachmentButton')) {
//                moveToNextStatus(event.target);
//            }
//        });
//    }

//    // If attachmentContainer exists, listen for new attachments
//    if (attachmentContainer) {
//        attachmentContainer.addEventListener("DOMNodeInserted", function () {
//            initializeAttachmentUpload();
//        });
//    }
//});
//document.addEventListener("DOMContentLoaded", function () {
//    console.log("attachmentUpload.js loaded successfully.");

//    // Attach event listener for the submit button(s)
//    const submitButtons = document.querySelectorAll(".submitAttachmentButton");

//    submitButtons.forEach(button => {
//        button.addEventListener("click", function () {
//            // استدعاء دالة التحقق من الملفات فقط عند الضغط على الزر
//            initializeAttachmentUpload();
//           moveToNextStatus(this); // تأكد من تنفيذ باقي المنطق بعد التحقق من الملفات
//        });
//    });

//    // إذا كانت العناصر مضافة ديناميكيًا، قم بإضافة listener بعد التحميل
//    const attachmentContainer = document.querySelector("#attachmentComponentContainer");
//    //if (attachmentContainer) {
//    //    attachmentContainer.addEventListener("DOMNodeInserted", function () {
//    //        // عند إضافة محتوى جديد، تأكد من تفعيل upload
//    //        initializeAttachmentUpload();
//    //    });
//    //}
//});
// دالة التحقق من رفع الملفات


// دالة التحقق من رفع الملفات
//function initializeAttachmentUpload() {
//    console.log("Initializing attachment upload...");

//    const allFileInputs = document.querySelectorAll('input[type="file"]');
//    const missingFiles = [];
//    const uploadedFiles = [];

//    allFileInputs.forEach(input => {
//        const isMandatory = input.getAttribute("data-mandatory") === "True";
//        const associatedTextInput = input.closest(".row").querySelector(".file-name-input");

//        let fileName;
//        if (input.getAttribute("data-filename") === "NewFile") {
//            fileName = associatedTextInput.value.trim() || "Unnamed File";
//            input.setAttribute("data-filename", fileName); // تحديث الاسم
//        } else {
//            fileName = input.getAttribute("data-filename") || "Unnamed File";
//        }

//        if (isMandatory) {
//            if (!input.value) {
//                missingFiles.push(fileName);
//            } else {
//                uploadedFiles.push({ file: input.files[0], fileName });
//            }
//        } else if (input.value) {
//            uploadedFiles.push({ file: input.files[0], fileName });
//        }
//    });

//    // إذا كانت هناك ملفات مفقودة
//    if (missingFiles.length > 0) {
//        alert(`يرجى رفع الملفات الإلزامية المطلوبة: \n - ${missingFiles.join("\n - ")}`);
//        return false;  // توقف عن التنفيذ
//    }

//    // عرض المرفقات التي تم رفعها
//    if (uploadedFiles.length > 0) {
//        uploadedFiles.forEach(({ file, fileName }) => {
//            createAttachmentCard(file, fileName);
//        });
//        alert("تم رفع الملفات وعرضها بنجاح.");
//        return true;
//    } else {
//        console.log("No files were uploaded.");
//    }
//}
//document.addEventListener("DOMContentLoaded", function () {
//    console.log("attachmentUpload.js loaded successfully.");

//    // Attach listener for dynamically loaded components
//    const attachmentContainer = document.querySelector("#attachmentComponentContainer");
//    const statusButtonsContainer = document.getElementById('statusButtons');

//    // Ensure that initializeAttachmentUpload is called when the page is loaded
//    initializeAttachmentUpload();

//    // Attach event listener for the submit button(s)
//    const submitButtons = document.querySelectorAll(".submitAttachmentButton");

//    // Add event listeners to each submit button
//    submitButtons.forEach(button => {
//        button.addEventListener("click", function () {
//            initializeAttachmentUpload(); // Perform the action for each button
//        });
//    });

//    // Add event listener to status buttons (to handle the status update)
//    if (statusButtonsContainer) {
//        statusButtonsContainer.addEventListener('click', function (event) {
//            if (event.target.matches('.submitAttachmentButton')) {
//                moveToNextStatus(event.target);
//            }
//        });
//    }

//    // If attachmentContainer exists, listen for new attachments and initialize upload
//    if (attachmentContainer) {
//        attachmentContainer.addEventListener("DOMNodeInserted", function () {
//            // Re-initialize the upload handling whenever new content is dynamically added
//            initializeAttachmentUpload();
//        });
//    }
//    document.querySelectorAll(".submitAttachmentButton").forEach(button => {
//        button.addEventListener("click", function () {
//            initializeAttachmentUpload(this); // Pass the clicked button to the function
//        });
//    });
//    // Handling submitButton dynamically to prevent duplicate event listeners
//    //document.querySelectorAll(".submitAttachmentButton").forEach(button => {
//    //    // Ensure each button gets initialized once, even if new buttons are dynamically added
//    //    if (!button.hasAttribute('data-initialized')) {
//    //        button.addEventListener("click", function () {
//    //            // Call the initialization and file upload validation before proceeding
//    //            initializeAttachmentUpload();
//    //            // Proceed with the status update
//    //            moveToNextStatus(this);
//    //        });
//    //        button.setAttribute('data-initialized', 'true');
//    //    }
//    //});
//});
//function initializeAttachmentUpload() {
//    console.log("Initializing attachment upload...");

//    // Select the submit buttons
//    const submitButtons = document.querySelectorAll(".submitAttachmentButton");

//    // Iterate through each button and add the click event listener
//    //submitButtons.forEach(submitButton => {
//    //    submitButton.addEventListener("click", function () {
//            const allFileInputs = document.querySelectorAll('input[type="file"]');
//            const missingFiles = [];
//            const uploadedFiles = [];

//            // Iterate through all file inputs and handle mandatory inputs
//            allFileInputs.forEach(input => {
//                const isMandatory = input.getAttribute("data-mandatory") === "True";
//                const associatedTextInput = input.closest(".row").querySelector(".file-name-input");

//                let fileName;
//                if (input.getAttribute("data-filename") === "NewFile") {
//                    fileName = associatedTextInput.value.trim() || "Unnamed File";
//                    input.setAttribute("data-filename", fileName); // Dynamically update the attribute
//                } else {
//                    fileName = input.getAttribute("data-filename") || "Unnamed File";
//                }

//                if (isMandatory) {
//                    if (!input.value) {
//                        missingFiles.push(fileName);
//                    } else {
//                        uploadedFiles.push({ file: input.files[0], fileName });
//                    }
//                } else if (input.value) {
//                    uploadedFiles.push({ file: input.files[0], fileName });
//                }
//            });

//            // If there are missing mandatory files, show an alert
//            if (missingFiles.length > 0) {
//                alert(`يرجى رفع الملفات الإلزامية المطلوبة: \n - ${missingFiles.join("\n - ")}`);
//                return;  // Stop the form submission
//            }

//            // Display uploaded attachments if any
//            if (uploadedFiles.length > 0) {
//                uploadedFiles.forEach(({ file, fileName }) => {
//                    createAttachmentCard(file, fileName);
//                });
//                alert("تم رفع الملفات وعرضها بنجاح.");
//            } else {
//                console.log("No files were uploaded.");
//            }
//    //    });
//    //});
//}
//function initializeAttachmentUpload() {
//    console.log("Initializing attachment upload...");

//    // const submitButton = document.querySelector("#submitAttachmentButton");
//    const submitButton = document.querySelector(".submitAttachmentButton");
//    if (!submitButton) {
//        console.warn("Submit attachment button not found!");
//        return;
//    }

//    // Add click event listener to the submit button
//    submitButton.addEventListener("click", function () {
//        const allFileInputs = document.querySelectorAll('input[type="file"]');
//        const allFileNames = document.querySelectorAll(".file-name-input");
//        console.log("All file inputs:", allFileNames);

//        console.log("All file inputs:", allFileInputs);

//        const missingFiles = [];
//        const uploadedFiles = [];

//        //// Iterate through all file inputs and handle mandatory inputs
//        allFileInputs.forEach(input => {
//            const isMandatory = input.getAttribute("data-mandatory") === "True";
//            //const fileName = input.getAttribute("data-filename") || "Unnamed File";
//            const associatedTextInput = input.closest(".row").querySelector(".file-name-input");

//            let fileName;
//            if (input.getAttribute("data-filename") === "NewFile") {
//                // Update the data-filename attribute with the new value
//                fileName = associatedTextInput.value.trim() || "Unnamed File";
//                input.setAttribute("data-filename", fileName); // Dynamically update the attribute
//            } else {
//                fileName = input.getAttribute("data-filename") || "Unnamed File";
//            }

//            if (isMandatory) {
//                if (!input.value) {
//                    missingFiles.push(fileName);
//                } else {
//                    // Push the file if it exists
//                    uploadedFiles.push({ file: input.files[0], fileName });
//                }
//            } else if (input.value) {
//                // Handle optional files
//                uploadedFiles.push({ file: input.files[0], fileName });
//            }
//        });
//        //allFileInputs.forEach((input, index) => {
//        //    const isMandatory = input.getAttribute("data-mandatory") === "True";
//        //    const fileNameInput = allFileNames[index];
//        //    const fileName = fileNameInput ? fileNameInput.value.trim() : "Unnamed File";

//        //    if (isMandatory && !input.value) {
//        //        missingFiles.push(fileName || "Unnamed Mandatory File");
//        //    } else if (input.value) {
//        //        uploadedFiles.push({ file: input.files[0], fileName });
//        //    }
//        //});
//        // Alert if there are missing mandatory files
//        if (missingFiles.length > 0) {
//            alert(`يرجى رفع الملفات الإلزامية المطلوبة: \n - ${missingFiles.join("\n - ")}`);
//            return;
//        }

//        // Display uploaded attachments
//        if (uploadedFiles.length > 0) {
//            uploadedFiles.forEach(({ file, fileName }) => {
//                createAttachmentCard(file, fileName);
//            });
//            alert("تم رفع الملفات وعرضها بنجاح.");
//        } else {
//            console.log("No files were uploaded.");
//        }
//    });
//}

//function initializeAttachmentUpload() {
//    console.log("Initializing attachment upload...");

//    const submitButton = document.querySelector("#submitAttachmentButton");
//    if (!submitButton) {
//        console.warn("Submit attachment button not found!");
//        return;
//    }

//    // Add click event listener to the submit button
//    submitButton.addEventListener("click", async function () {
//        const allFileInputs = document.querySelectorAll('input[type="file"]');
//        console.log("All file inputs:", allFileInputs);
//        allFileInputs.forEach(input => {
//            console.log("Input Name:", input.name);
//            console.log("Input Class:", input.className);
//            console.log("Data Mandatory:", input.getAttribute("data-mandatory"));
//            console.log("Filename:", input.getAttribute("data-filename"));
//        });




//        // Initialize an array to store missing mandatory files
//        const missingFiles = [];

//        // Iterate through all file inputs and handle mandatory inputs
//        allFileInputs.forEach(input => {
//          //  const isMandatory = input.getAttribute("data-mandatory").value === "true";
//            const isMandatory = input.getAttribute("data-mandatory") === "True"; // Match capitalized "True"
//            if (isMandatory) {
//                console.log("Mandatory Input Name:", input.name);
//                console.log("Filename:", input.getAttribute("data-filename"));

//                // Check if the input is empty
//                if (!input.value) {
//                    const fileName = input.getAttribute("data-filename") || "ملف غير مسمى";
//                    missingFiles.push(fileName);
//                }
//            }
//        });

//        // Check if there are missing files
//        if (missingFiles.length > 0) {
//            alert(`يرجى رفع الملفات الإلزامية المطلوبة: \n - ${missingFiles.join("\n - ")}`);
//            return;
//        } else {
//            console.log("All mandatory files have been uploaded.");

//        }

//        const form = document.querySelector("#attachmentUploadForm");
//        if (!form) {
//            console.error("Attachment upload form not found!");
//            return;
//        }

//        const formData = new FormData(form);
//        displayAttachments(formData);
//        alert("تم رفع الملفات وعرضها بنجاح.");
//    });
//}
//function initializeAttachmentUpload() {
//    console.log("Initializing attachment upload...");

//    const submitButton = document.querySelector("#submitAttachmentButton");
//    if (!submitButton) {
//        console.warn("Submit attachment button not found!");
//        return;
//    }

//    // Add click event listener to the submit button
//    submitButton.addEventListener("click", function () {
//        const allFileInputs = document.querySelectorAll('input[type="file"]');
//        console.log("All file inputs:", allFileInputs);

//        const missingFiles = [];
//        const uploadedFiles = [];

//        // Iterate through all file inputs and handle mandatory inputs
//        allFileInputs.forEach(input => {
//            const isMandatory = input.getAttribute("data-mandatory") === "true"; // Ensure matching string comparison
//            const fileName = input.getAttribute("data-filename") || "Unnamed File";

//            if (isMandatory) {
//                if (!input.value) {
//                    missingFiles.push(fileName);
//                } else {
//                    // Push the file if it exists
//                    uploadedFiles.push({ file: input.files[0], fileName });
//                }
//            } else if (input.value) {
//                // Handle optional files
//                uploadedFiles.push({ file: input.files[0], fileName });
//            }
//        });

//        // Alert if there are missing mandatory files
//        if (missingFiles.length > 0) {
//            alert(`يرجى رفع الملفات الإلزامية المطلوبة: \n - ${missingFiles.join("\n - ")}`);
//            return;
//        }

//        // Display uploaded attachments
//        if (uploadedFiles.length > 0) {
//            uploadedFiles.forEach(({ file, fileName }) => {
//                createAttachmentCard(file, fileName);
//            });
//            alert("تم رفع الملفات وعرضها بنجاح.");
//        } else {
//            console.log("No files were uploaded.");
//        }
//    });
//}
/**
 * Validate mandatory files.
 * @param {NodeList} mandatoryFiles - List of mandatory file input elements.
 * @returns {Array} - List of missing file names.
 */


/**
 * Display uploaded attachments dynamically.
 * @param {FormData} formData - FormData object containing uploaded files.
 */
//document.addEventListener("DOMContentLoaded", function () {
//    let fileIndex = 0; // Track the index of the added files

//    // Add a new file input when the add button is clicked
//    document.querySelector("#addAttachmentField").addEventListener("click", function () {
//        const attachmentFields = document.querySelector("#attachmentFields");

//        // Create a new input field container
//        const newFieldRow = document.createElement("div");
//        newFieldRow.classList.add("row", "mb-3");

//        // Generate the HTML for the new input field
//        newFieldRow.innerHTML = `
//        <div class="col-md-5">
//            <label>اسم الملف</label>
//            <input type="text"
//                   class="form-control file-name-input"
//                   placeholder="أدخل اسم الملف" />
//        </div>
//        <div class="col-md-5">
//            <label>رفع ملف</label>
//            <input type="file"
//                   name="AttachmentFiles-${fileIndex}"
//                   class="form-control file-input"
//                   data-mandatory="false"
//                   data-filename="NewFile" />
//        </div>
//        <div class="col-md-2">
//            <button type="button" class="btn btn-danger m-3 removeField">إزالة</button>
//        </div>
//    `;

//        // Append the new field to the attachment fields container
//        attachmentFields.appendChild(newFieldRow);

//        // Increment the file index for the next input field
//        fileIndex++;
//    });

//    // Use event delegation to remove a field when the remove button is clicked
//    document.querySelector("#attachmentFields").addEventListener("click", function (event) {
//        if (event.target.classList.contains("removeField")) {
//            const fieldToRemove = event.target.closest(".row");
//            fieldToRemove.remove();
//        }
//    });
//});

// Create an attachment card dynamically
function initializeAttachmentUpload() {
    console.log("Initializing attachment upload...");

    const allFileInputs = document.querySelectorAll('input[type="file"]');
    const missingFiles = [];
    const uploadedFiles = [];

    // تحقق من كل ملف
    for (let input of allFileInputs) {
        const isMandatory = input.getAttribute("data-mandatory") === "True";
        const associatedTextInput = input.closest(".row").querySelector(".file-name-input");
        const fieldname = input.getAttribute("data-fieldname");
        let fileName;
        if (input.getAttribute("data-filename") === "NewFile") {
            fileName = associatedTextInput.value.trim() || "Unnamed File";
            input.setAttribute("data-filename", fileName); // تحديث الاسم
        } else {
            fileName = input.getAttribute("data-filename") || "Unnamed File";
        }

        if (isMandatory && !input.value) {
            missingFiles.push(fileName); // إذا كان الملف مفقودًا، أضفه إلى القائمة
            break;  // التوقف عن التحقق بعد العثور على الملف المفقود
        } else if (input.value) {
            const isMandatory = input.getAttribute("data-mandatory") === "True";
            const fieldname = input.getAttribute("data-fieldname");

            //console.log(isMandatoryFlag);
            console.log(fieldname);

            //uploadedFiles.push({ file: input.files[0], fileName, isMandatory: isMandatoryFlag });
            uploadedFiles.push({
                file: input.files[0],
                fileName,
                isMandatory,
                fieldname
            });
        }
    }

    // إذا كانت هناك ملفات مفقودة
    if (missingFiles.length > 0) {
        alert(`يرجى رفع الملفات الإلزامية المطلوبة: \n - ${missingFiles.join("\n - ")}`);
        return false;  // توقف عن التنفيذ إذا كانت هناك ملفات مفقودة
    }

    // عرض المرفقات التي تم رفعها
    if (uploadedFiles.length > 0) {
        uploadedFiles.forEach(({ file, fileName }) => {
            createAttachmentCard(file, fileName);
        });
        alert("تم رفع الملفات وعرضها بنجاح.");
        return true;  // إرجاع true إذا كانت الملفات قد تم رفعها بنجاح
    } else {
        console.log("No files were uploaded.");
        return true; // إذا لم يتم رفع أي ملفات
    }
}
function validateMandatoryFiles(mandatoryFiles) {
    const missingFiles = [];
    mandatoryFiles.forEach(fileInput => {
        if (!fileInput.value) {
            const fileName = fileInput.getAttribute("data-filename") || "ملف غير مسمى";
            missingFiles.push(fileName);
        }
    });
    return missingFiles;
}
function createAttachmentCard(file, fileName)
{

    const attachmentSection = document.getElementById('attachmentsSection');

    if (!attachmentSection) {
        console.error("❌ Error: 'attachmentsSection' not found in the DOM.");
        return;
    }

    const newCard = document.createElement('div');
    newCard.classList.add('card', 'shadow-sm', 'pt-4', 'mb-6', 'mb-xl-9');

    // Generate a unique ID for the collapsible section
    const uniqueId = `kt_docs_card_collapsible_${Date.now()}`;

    //console.log("📂 File received:", file);

    // Check if `file` is wrapped in an object
    const fileToUse = file instanceof File ? file : file.file;

    if (!(fileToUse instanceof File)) {
        console.error("❌ Error: fileToUse is not a valid File object", fileToUse);
        return;
    }

    const fileURL = URL.createObjectURL(fileToUse);

    newCard.innerHTML = `
        <div class="card-header collapsible cursor-pointer rotate" data-bs-toggle="collapse" data-bs-target="#${uniqueId}">
            <div class="card-title flex-column">
                <div class="d-inline-flex align-items-center">
                    <input type="checkbox" 
                           name="attachmentsToUpload" 
                           id="checkbox_${uniqueId}" 
                           value="${fileName}" 
                           class="form-check-input me-2" 
                           checked>
                    <label for="checkbox_${uniqueId}" class="mb-1 text-muted">${fileName}</label>
                </div>
                <span class="d-inline-block position-relative ms-2">
                    <span class="d-inline-block mb-2 fw-bold">${fileName}</span>
                    <span class="d-inline-block position-absolute h-3px bottom-0 end-0 start-0 bg-primary translate rounded"></span>
                </span>
            </div>
            <div class="card-toolbar rotate-180">
                <i class="ki-duotone ki-down fs-1"></i>
            </div>
        </div>
        <div id="${uniqueId}" class="collapse">
            <div class="card-body">
                <iframe src="${fileURL}" style="width: 100%; height: 500px;"></iframe>
            </div>
            <div class="card-footer">
                <a href="${fileURL}" class="btn btn-light-primary btn-sm" target="_blank">
                    <i class="ki-outline ki-book-open fs-3"></i> فتح في شاشة منفصلة
                </a>
            </div>
        </div>
    `;

    console.log("🆕 New attachment card:", newCard.outerHTML);

    // ✅ Ensure section is visible
    attachmentSection.style.display = "block";

    // ✅ Append new card
    try {
        attachmentSection.appendChild(newCard);
        console.log("✅ Attachment added successfully!");
    } catch (error) {
        console.error("❌ Error appending new card:", error);
    }
}


document.addEventListener("DOMContentLoaded", function () {
    let fileIndex = 0; // Track the index of the added files

    // Open the modal when the "+ إضافة مرفقات أخري" button is clicked
    //document.querySelector("#addAttachmentField").addEventListener("click", function () {
    //    const attachmentModal = new bootstrap.Modal(document.getElementById('attachmentModal'));
    //    attachmentModal.show(); // Show modal
    //});
    
    // Add a new file input when the "إضافة حقل" button is clicked inside the modal
    document.querySelector("#addMoreFields").addEventListener("click", function () {
        const attachmentFields = document.querySelector("#attachmentFieldsModal");

        // Create a new input field container
        const newFieldRow = document.createElement("div");
        newFieldRow.classList.add("row", "mb-3");

        // Generate the HTML for the new input field
        newFieldRow.innerHTML = `
      <div class="col-md-5">
        <label>اسم الملف</label>
        <input type="text" class="form-control file-name-input" placeholder="أدخل اسم الملف" />
      </div>
      <div class="col-md-5">
        <label>رفع ملف</label>
        <input type="file" name="AttachmentFiles-${fileIndex}" class="form-control file-input" data-mandatory="false" data-filename="NewFile" />
      </div>
      <div class="col-md-2">
        <button type="button" class="btn btn-danger m-3 removeField">إزالة</button>
      </div>
    `;

        // Append the new field to the attachment fields container inside the modal
        attachmentFields.appendChild(newFieldRow);

        // Increment the file index for the next input field
        fileIndex++;
    });
    document.querySelector("#attachmentFieldsModal").addEventListener("click", function (event) {
        if (event.target.classList.contains("removeField")) {
            const fieldToRemove = event.target.closest(".row");
            fieldToRemove.remove();
        }
    });

    // Handle the form submission when "إرسال" button is clicked inside the modal
    
       
        document.querySelector("#submitAttachmentButton").addEventListener("click", function () {
            // Step 1: Validate required attachments
            const missingFiles = validateAttachmentFiles();
            if (missingFiles.length > 0) {
                alert(`يرجى رفع الملفات الإلزامية المطلوبة: \n - ${missingFiles.join("\n - ")}`);
                return; // Stop submission
            }

            // Step 2: Get form data
            const form = document.querySelector("#attachmentUploadForm");

            const formData = new FormData(form);
            //let uploadedFiles = [];
            const fileInputs = document.getElementsByClassName("file-input").value; 
            const fileNameInputs = document.getElementsByClassName("file-name-input");            
            console.log(fileNameInputs);
            console.log(fileInputs);

            // Step 3: Retrieve uploaded files correctly
            //const uploadedFiles = getUploadedFiles(formData);
            const uploadedFiles = getUploadedFiles();

            // Debugging: Check final uploaded files
            console.log("Final uploadedFiles array:", uploadedFiles);

            // Step 4: If files exist, create preview cards
            if (uploadedFiles.length > 0) {
                uploadedFiles.forEach(({ Files, filename }) => createAttachmentCard(Files, filename));
                alert("تم رفع الملفات وعرضها بنجاح.");
                saveFilesToServer(uploadedFiles);
            } else {
                console.log("❌ No files were uploaded.");
            }
        });
    //حفظ الملفات الأخري
    function loadPageContent() {
        console.log("Current URL:", window.location.href);
        // باقي الكود الذي يقوم بتحميل أو تغيير المحتوى
    }
    function saveFilesToServer(uploadedFiles) {
        const formData = new FormData();

        const relativePath = "Uploads"; 

        console.log(uploadedFiles);
        // Append each file to FormData
        //uploadedFiles.forEach(({ file, fileName }) => {
        //    formData.append("AttachmentFiles[]", file);
        //    formData.append("FileNames[]", fileName);
        //  /*  formData.append("relativePath", relativePath); */

        //});
        //formData.append("uploadedFiles[]", uploadedFiles);
        formData.append("RequestId", document.querySelector("input[name='RequestId']").value);
        formData.append("ReqNo", document.querySelector("input[name='ReqNo']").value);


        uploadedFiles.forEach(({ Files, filename, isMandatory, fieldName }, index) => {
            formData.append(`files[${index}].Files`, Files); 
            formData.append(`files[${index}].filename`, filename); 
            formData.append(`files[${index}].IsMandatory`, isMandatory); 
            formData.append(`files[${index}].FieldName`, fieldName);
        });

        // Append each file to FormData
        //uploadedFiles.forEach(({ file }) => {
        //    formData.append("AttachmentFiles[]", file);

        //});

        console.log("📤 Sending files to the server...");
        for (let pair of formData.entries()) {
            console.log(pair[0] + ': ' + pair[1]);
        }
        const uploadUrl = document.getElementById("uploadUrl")?.value || "/Admin/Tourism/SaveFile";

        // Send AJAX request to backend
        //fetch("/Admin/Tourism/SaveFile", {
        //    method: "POST",
        //    body: formData
        //})
        fetch(uploadUrl, {
            method: "POST",
            body: formData
        })
            .then(response => response.json()) // Adjust based on your backend response type
            .then(data => {
                console.log("✅ Files saved successfully:", data);
                alert("تم حفظ الملفات بنجاح!");
            })
            .catch(error => {
                console.error("❌ Error saving files:", error);
                alert("حدث خطأ أثناء حفظ الملفات.");
            });
    }
    /**
     * Extracts all uploaded files from FormData
     * @param {FormData} formData 
     * @returns {Array} List of files with names
     */
    function getUploadedFiles() {
        let uploadedFiles = [];

        // Debugging: Log FormData before processing
        console.log("FormData Entries:");
        //for (const [key, value] of formData.entries()) {
        //    console.log(`Key: ${key}, Value:`, value);
        //    if (key.startsWith("AttachmentFiles")) {
        //        uploadedFiles.push({ file: value, fileName: value.name });
               
        //    }
        //}
     
        const fileInputs = document.getElementsByClassName("file-input"); // Select all file inputs
        const fileNameInputs = document.getElementsByClassName("file-name-input"); // Select all name inputs

        for (let i = 0; i < fileInputs.length; i++) {
            const Files = fileInputs[i].files[0]; // Get the file
            const filename = fileNameInputs[i].value.trim(); // Get the entered file name
            const isMandatory = fileInputs[i].getAttribute("data-mandatory") === "true"; // Get mandatory status
            const fieldName = fileInputs[i].getAttribute("data-fieldname") || "";


            
            if (Files) {
                uploadedFiles.push({ Files, filename: filename, isMandatory, fieldName /*|| file.name*/ });
            }
        }

        console.log("Final uploadedFiles array:", uploadedFiles);
        return uploadedFiles;
       
    }


});

// Validate files - checking mandatory files
function validateAttachmentFiles() {
    const mandatoryFiles = document.querySelectorAll('input[data-mandatory="true"]');
    let missingFiles = [];

    mandatoryFiles.forEach(fileInput => {
        if (!fileInput.value) {
            missingFiles.push(fileInput.getAttribute("data-filename"));
        }
    });

    return missingFiles;
}

