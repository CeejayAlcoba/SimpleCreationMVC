const handleGetScript = async (baseConnectionString, comparisonConnectionString) => {

    let newBaseConnectionString = encodeURI(baseConnectionString);
    let newComparisonConnectionString = encodeURI(comparisonConnectionString)
    console.log(newBaseConnectionString, newComparisonConnectionString)
    return await ajaxInstance({
        method: "GET",
        url: `api/database-comparer?baseConnectionString=${newBaseConnectionString}&comparisonConnectionString=${newComparisonConnectionString}`
    });
}

$(document).ready(function () {
    handleOnClickGenerateButton();
    let baseConnectionString = localStorage.getItem("baseConnectionString") ?? "";
    let comparisonConnectionString = localStorage.getItem("comparisonConnectionString" ?? "");
    $(`[data-input="base-connection-string"]`).val(baseConnectionString)
    $(`[data-input="comparison-connection-string"]`).val(comparisonConnectionString)
    
});

const handleOnClickGenerateButton = () => {
    $('#generate-script-btn').on('click', async function () {
        const baseConnectionString = $('[data-input="base-connection-string"]').val();
        const comparisonConnectionString = $('[data-input="comparison-connection-string"]').val();

        if (!baseConnectionString || !comparisonConnectionString) {
            alert('Please fill in both connection strings!');
            return;
        }

        const sqlScripts = await handleGetScript(baseConnectionString, comparisonConnectionString);

        if (!sqlScripts || !Array.isArray(sqlScripts) || sqlScripts.length === 0) {
            alert('No scripts found!');
            return;
        }

        // Create a container to hold all scripts
        const scriptContent = sqlScripts.map((script, index) => `
            <div class="script-item col-lg-10 mb-3 card">
                <h6>Script ${index + 1}:</h6>
                <pre id="script-${index}">${script}</pre>
                <button class="btn btn-secondary btn-sm copy-btn" data-script-id="script-${index}"><i class="bi bi-clipboard-check-fill ml-1"></i>Copy</button>
            </div>
        `).join('');

        // Append to the script container
        $('#script-container').html(`
                <div class="row d-flex justify-content-center">
                  <h5 class="col-lg-8">Generated SQL Scripts:</h5>
                  <button class="btn btn-primary btn-sm col-lg-2" id="copy-all-btn">Copy All Scripts</button>
                </div>
              
                ${scriptContent}
                
        `);

        // Bind individual copy buttons
        $('.copy-btn').on('click', function () {
            const scriptId = $(this).data('script-id');
            const scriptContent = $(`#${scriptId}`).text();
            copyToClipboard(scriptContent);
            alert('Script copied to clipboard!');
        });

        // Copy all scripts functionality
        $('#copy-all-btn').on('click', function () {
            const allScripts = sqlScripts.join('\n\n');
            copyToClipboard(allScripts);
            alert('All SQL scripts copied to clipboard!');
        });
    });
};
$(`[data-input="base-connection-string"]`).keyup((e) => {
    localStorage.setItem("baseConnectionString", e.target.value)
})
$(`[data-input="comparison-connection-string"]`).keyup((e) => {
    localStorage.setItem("comparisonConnectionString", e.target.value)
});



// Helper function to copy text to clipboard
const copyToClipboard = (text) => {
    const textarea = $('<textarea>');
    $('body').append(textarea);
    textarea.val(text).select();
    document.execCommand('copy');
    textarea.remove();
}
