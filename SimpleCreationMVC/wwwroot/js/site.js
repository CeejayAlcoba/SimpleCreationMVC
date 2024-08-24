$(_ => {
    generateDownloadButtons();
    handleConnectionStringOnChange();
    handleClickGenerateBtn();
    handleClickSystemDownloadBtn();
    isLoading(false);
})

const ajaxInstance = async (ajaxProps) => {
    isLoading(true);
    const data = await $.ajax({ ...ajaxProps, contentType: "application/json; charset=utf-8" });
    isLoading(false);
    return data;
}

//---APIs---//
const getTableShemas = async (connectionString) => {
    const encodedConnectionString = getConnectionStringEncodedUri();
    return await ajaxInstance({
        method: "GET",
        url: `api/sql/table-schema?connectionString=${encodedConnectionString}`
    })
}

const downloadProject = async() => {
   await window.open(`api/download-project`)
}

const downloadDapperQuery = async (tables) => {
    await ajaxInstance({
        method: "POST",
        url: `api/dapper-query/create?connectionString=${getConnectionStringEncodedUri()}`,
        data: JSON.stringify(tables ?? tableSchemas)
    })
    await downloadProject();
}

const downloadDapperProcedure = async (tables) => {
    await ajaxInstance({
        method: "POST",
        url: `api/dapper-stored-procedure/create?connectionString=${getConnectionStringEncodedUri()}`,
        data: JSON.stringify(tables ?? tableSchemas)
    })
    await downloadProject();
}

const downloadEFCore = async (tables) => {
    await ajaxInstance({
        method: "POST",
        url: `api/ef-core/create?connectionString=${getConnectionStringEncodedUri()}`,
        data: JSON.stringify(tables ?? tableSchemas)
    })
    await downloadProject();
}

const downloadModel = async (tables) => {
    await ajaxInstance({
        method: "POST",
        url: `api/model/create?connectionString=${getConnectionStringEncodedUri()}`,
        data: JSON.stringify(tables ?? tableSchemas)
    })
    await downloadProject();
}

const downloadTsTypes = async (tables) => {
    await ajaxInstance({
        method: "POST",
        url: `api/front-end/ts-types/create?connectionString=${getConnectionStringEncodedUri()}`,
        data: JSON.stringify(tables ?? tableSchemas)
    })
    await downloadProject();
}

const downloadJsClasses = async (tables) => {
    await ajaxInstance({
        method: "POST",
        url: `api/front-end/js-classes/create?connectionString=${getConnectionStringEncodedUri()}`,
        data: JSON.stringify(tables ?? tableSchemas)
    })
    await downloadProject();
}

const downloadSystemPublished= () => {
    window.open(`api/system/download`)
}


//---INPUT COMPONNET---//

const getConnectionStringEncodedUri = () => encodeURI(localStorage.getItem("connectionString") ?? $(`[data-input="connectionString"]`).val());
const getConnectionStringDecodedUri = () => decodeURI(getConnectionStringEncodedUri());

const handleConnectionStringOnChange = () => {
    $(`[data-input="connectionString"]`).val(getConnectionStringDecodedUri());

    $(`[data-input="connectionString"]`).on("keyup", (e) => {

        localStorage.setItem("connectionString", e.target.value);
        disabledDownloadButtons();
    })
}


//---BUTTON COMPONENT---//
let downloadBottons = [
    {
        label: "Dapper Query Download",
        onClick: `downloadDapperQuery()`,

    },
    {
        label: "Dapper Stored Procedure Download",
        onClick: `downloadDapperProcedure()`,

    },
    {
        label: "EF Core Download",
        onClick: `downloadEFCore()`,

    },
    {
        label: "Model Only Download",
        onClick: `downloadModel()`,

    },
    {
        label: "TypeScript Types Only Download",
        onClick: `downloadTsTypes()`,

    },
    {
        label: "JavaScript Classes Only Download",
        onClick: `downloadJsClasses()`,
    },
];

const disabledDownloadButtons = () => {
    const isDisabled = !getConnectionStringEncodedUri() || tableSchemas.length == 0;
    $(".download-btn").prop("disabled", isDisabled)
}

const handleClickGenerateBtn = () => {
    $("#generate-btn").on("click", async () => {
        await handleGenerateTableSchemas();
        disabledDownloadButtons();
    })
}

const handleClickSystemDownloadBtn = () => {
    $("#system-download-btn").on("click", async () => {
        await downloadSystemPublished();
    })
}

const generateDownloadButtons = () => {
    downloadBottons.map(btn => {
        $("#botton-download-container").append(`
        <button type="button" class="download-btn btn btn-${btn.btnColor ?? 'primary'} col-lg-5" onClick=` + `${btn.onClick}` + `> ${btn.label} </button>
    `);
    })
    disabledDownloadButtons();
}



//---TABLE SCHEMA COMPONENTS---//

let tableSchemas = [];

let headers = [
    {
        key: 'columN_NAME',
        label: 'Column'
    },
    {
        key: 'datA_TYPE',
        label: 'Data Type'
    }
];

let checkBoxes = [
    {
        key: 'isControllerFileAllowed',
        label: 'Controller File',
    },
    {
        key: 'isServiceFileAllowed',
        label: 'Service File',
    },
    {
        key: 'isRepositoryFileAllowed',
        label: 'Repository File',
    },
    {
        key: 'isModelFileAllowed',
        label: 'Model File',
    },
]

const handleGenerateTableSchemas = async (tableSchemaData) => {
    const connectionString = getConnectionStringEncodedUri();
        tableContainer = $(`#table-schema-container`);

    tableContainer.empty();
    tableSchemas = tableSchemaData ?? await getTableShemas(connectionString);
    tableSchemas.map(schema => {
        const table = tableSchemaComponent({ title: schema.tablE_NAME, schema, headers });
        tableContainer.append(table);
    })
}

const handleCheckbox = (tableName, key) => {
    let table = tableSchemas.find(t => t.tablE_NAME == tableName);
    table[key] = !table[key];
}

const handleRemoveTable = (tableName) => {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            Swal.fire({
                position: "center",
                icon: "success",
                title: "Successfully deleted",
                showConfirmButton: false,
                timer: 1500
            });
            const filteredTable =  tableSchemas.filter(t => t.tablE_NAME !== tableName);
            handleGenerateTableSchemas(filteredTable);
        }
    });
   
}

const tableSchemaComponent = ({ title, schema, headers = [] }) => {
    const { columns } = schema;
    const headersComponent = headers.reduce((prev, header) => prev += ` <th scope="col">${header.label}</th>`, "");
    const rowsComponent = columns.reduce((prevRow, row) => {
        prevRow += `<tr>`;
        const columnsComponent = headers.reduce((prevCol, col) =>
            prevCol += `<td>${row[col.key]}</td>`
            , "")

        return prevRow += `${columnsComponent}</tr>`;
    }, "");

    const checkBoxesComponent = checkBoxes.reduce((prev, checkBox) => {
        const isChecked = schema[checkBox.key] ? "checked" : "";
        return prev += `<div class="col-md-4">
                            <input type="checkbox" ${isChecked} onchange='handleCheckbox("${schema.tablE_NAME}", "${checkBox.key}")'>
                            <label>${checkBox.label}</label>
                        </div>`
    }, "")

    return `
         <div class="col-lg-5 card p-2">
            <div class="d-flex justify-content-between">
                <h5>${title}</h5>
                <i class="bi bi-trash-fill text-danger" onClick='handleRemoveTable("${schema.tablE_NAME}")'></i>
           </div>
           <div style="max-height:310px; overflow:auto"> 
                 <table class="table" >
                      <thead>
                        <tr>
                         ${headersComponent}
                        </tr>
                      </thead>
                      <tbody>
                       ${rowsComponent}
                      </tbody>
                 </table>
           </div>
           <div class="row">
             ${checkBoxesComponent}
           </div>
         </div>`
}


//---LOADING PAGE SPINNER---//

const isLoading = async (isloading) => {
    await $('.loading-overlay').css('display', isloading ? '' :'none')
}



