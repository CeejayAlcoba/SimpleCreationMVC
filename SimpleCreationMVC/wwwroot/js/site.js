$(_ => {
    generateDownloadOptions();
    handleOnClickDownloadButton();
    disabledDownloadButton();
    disabledPrintButton();

    handleConnectionStringOnChange();
    handleClickGenerateBtn();
    handleClickPrintButton();
    handleClickSystemDownloadBtn();

    handleTableFilterOnChange();
    isLoading(false);
})

const ajaxInstance = async (ajaxProps) => {
    try {

        isLoading(true);
        const data = await $.ajax({ ...ajaxProps, contentType: "application/json; charset=utf-8" });
        return data;
    }
    catch (e) {
        Swal.fire({
            icon: "error",
            title: "Oops...",
            text: e?.responseText ?? "Something went wrong!",
        });
    }
    finally {
        isLoading(false);
    }
}

//---APIs---//
const getTableShemas = async () => {
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
        data: JSON.stringify(tables ?? tempTableSchemas)
    })
    await downloadProject();
}

const downloadDapperProcedure = async (tables) => {
    await ajaxInstance({
        method: "POST",
        url: `api/dapper-stored-procedure/create?connectionString=${getConnectionStringEncodedUri()}`,
        data: JSON.stringify(tables ?? tempTableSchemas)
    })
    await downloadProject();
}

const downloadEFCore = async (tables) => {
    await ajaxInstance({
        method: "POST",
        url: `api/ef-core/create?connectionString=${getConnectionStringEncodedUri()}`,
        data: JSON.stringify(tables ?? tempTableSchemas)
    })
    await downloadProject();
}

const downloadStoredProcedure = async (tables) => {
    await ajaxInstance({
        method: "POST",
        url: `api/sql/stored-procedure/create?connectionString=${getConnectionStringEncodedUri()}`,
        data: JSON.stringify(tables ?? tempTableSchemas)
    })
    await downloadProject();
}

const downloadModel = async (tables) => {
    await ajaxInstance({
        method: "POST",
        url: `api/model/create?connectionString=${getConnectionStringEncodedUri()}`,
        data: JSON.stringify(tables ?? tempTableSchemas)
    })
    await downloadProject();
}

const downloadController = async (tables) => {
    await ajaxInstance({
        method: "POST",
        url: `api/controller/create?connectionString=${getConnectionStringEncodedUri()}`,
        data: JSON.stringify(tables ?? tempTableSchemas)
    })
    await downloadProject();
}

const downloadRepository = async (tables) => {
    await ajaxInstance({
        method: "POST",
        url: `api/repository/create?connectionString=${getConnectionStringEncodedUri()}`,
        data: JSON.stringify(tables ?? tempTableSchemas)
    })
    await downloadProject();
}

const downloadService = async (tables) => {
    await ajaxInstance({
        method: "POST",
        url: `api/service/create?connectionString=${getConnectionStringEncodedUri()}`,
        data: JSON.stringify(tables ?? tempTableSchemas)
    })
    await downloadProject();
}

const downloadTsTypes = async (tables) => {
    await ajaxInstance({
        method: "POST",
        url: `api/front-end/ts-types/create?connectionString=${getConnectionStringEncodedUri()}`,
        data: JSON.stringify(tables ?? tempTableSchemas)
    })
    await downloadProject();
}

const downloadJsClasses = async (tables) => {
    await ajaxInstance({
        method: "POST",
        url: `api/front-end/js-classes/create?connectionString=${getConnectionStringEncodedUri()}`,
        data: JSON.stringify(tables ?? tempTableSchemas)
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
        disabledDownloadButton();
    })
}


//---DOWNLOAD COMPONENTS---//
let downloadOptions = [
    {
        label: "Dapper Query Project",
        onDownload:()=> downloadDapperQuery(),

    },
    {
        label: "Dapper Stored Procedure Project",
        onDownload: () => downloadDapperProcedure(),

    },
    {
        label: "EF Core Project",
        onDownload: () => downloadEFCore(),

    },
    {
        label: "CRUD Stored Procedure Only",
        onDownload: () => downloadStoredProcedure()

    },
    {
        label: "Model Only",
        onDownload: () => downloadModel(),

    },
    {
        label: "Controller Only",
        onDownload: () => downloadController()

    },
    {
        label: "Repository Only",
        onDownload: () => downloadRepository()

    },
    {
        label: "Service Only",
        onDownload: () => downloadService()

    },
    {
        label: "TypeScript Types Only",
        onDownload: () => downloadTsTypes(),

    },
    {
        label: "JavaScript Classes Only",
        onDownload: () => downloadJsClasses(),
    },
];

const generateDownloadOptions = () => {
    downloadOptions.map(options => {
        $(`#select-option-download`).append(
            `<option value="${options.label}">${options.label}</option>`
        )
    })
    
}

const handleOnClickDownloadButton = () => {
    $(`#download-btn`).on('click', _ => {
        const downloadVal = $(`#select-option-download`).val();
        downloadOptions.find(option => option.label == downloadVal).onDownload();
    })
}

const handleClickPrintButton = () => {
    $('#print-btn').on('click', function () {
        const printWindow = window.open('', '_blank');
        printWindow.document.open();
        printWindow.document.write(generatePrintContent());
        printWindow.document.close();

        printWindow.focus();
        printWindow.print();
        printWindow.close();
    });
}

const handleClickSystemDownloadBtn = () => {
    $("#system-download-btn").on("click", async () => {
        await downloadSystemPublished();
    })
}

const disabledDownloadButton = () => {
    const isDisabled = !getConnectionStringEncodedUri() || tempTableSchemas.length == 0;
    $("#download-btn").prop("disabled", isDisabled)
}

const disabledPrintButton = () => {
    const isDisabled = !getConnectionStringEncodedUri() || tempTableSchemas.length == 0;
    $("#print-btn").prop("disabled", isDisabled)
}

const handleClickGenerateBtn = () => {
    $("#generate-btn").on("click", async () => {
        await handleGenerateTempTableSchemas();
        disabledDownloadButton();
        disabledPrintButton();
        generateTableFilterOptions();
    })
}


//---TABLE FILTER SELECT OPTION COMPONENTS---//
const handleTableFilterOnChange = () => {
    $(`#table-filter-option`).on('change', _ => {
        tempTableSchemas = tableSchemas;
        const filterVal = $(`#table-filter-option`).val();
        if (filterVal == "all") {
            handleGenerateTempTableSchemas(tempTableSchemas);
        }
        else {
            const filteredTable = tempTableSchemas.filter(t => t.tablE_NAME == filterVal);
            handleGenerateTempTableSchemas(filteredTable);
        }
    })

}

const generateTableFilterOptions = () => {
    $(`#table-filter-option`).empty();
    $(`#table-filter-option`).append(`
            <option value="all">All</option> `
    );
    tempTableSchemas.map(table => {
        $(`#table-filter-option`).append(
            `<option value="${table.tablE_NAME}">${table.tablE_NAME}</option>`
        )
    })
   
}



//---TABLE SCHEMA COMPONENTS---//
let tableSchemas = [];
let tempTableSchemas = [];

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

const handleGenerateTempTableSchemas = async (tableSchemaData) => {
    const connectionString = getConnectionStringEncodedUri();
        tableContainer = $(`#table-schema-container`);

    tableContainer.empty();
    if (tableSchemaData) {
        tempTableSchemas = tableSchemaData
    }
    else {
        tableSchemas = await getTableShemas(connectionString);
        tempTableSchemas = tableSchemas
        
    }
    tempTableSchemas.map(schema => {
        const table = tableSchemaComponent({ title: schema.tablE_NAME, schema, headers });
        tableContainer.append(table);
    })
}

const handleCheckbox = (tableName, key) => {
    let table = tempTableSchemas.find(t => t.tablE_NAME == tableName);
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
            const filteredTable = tableSchemas.filter(t => t.tablE_NAME !== tableName);
            handleGenerateTempTableSchemas(filteredTable);
            generateTableFilterOptions();
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
       </div>`
}


//---LOADING PAGE SPINNER---//
const isLoading = async (isloading) => {
    await $('.loading-overlay').css('display', isloading ? '' :'none')
}
function generatePrintContent() {
    let html = `<html><head><title>Print</title>
      <style>
        body {
          font-family: sans-serif;
          padding: 20px;
        }
        .table-grid {
          display: flex;
          flex-wrap: wrap;
          gap: 20px;
        }
        .table-item {
          width: calc(50% - 10px); /* two columns */
          box-sizing: border-box;
        }
        table {
          width: 100%;
          border-collapse: collapse;
          margin-top: 10px;
        }
        th, td {
          border: 1px solid #000;
          padding: 6px;
          font-size: 14px;
        }
        th {
          background-color: #f0f0f0;
        }
        h2 {
          margin: 0;
          font-size: 16px;
        }
        @media print {
          body {
            margin: 0;
          }
        }
      </style>
      </head><body>
      <div class="table-grid">`;

    tempTableSchemas.forEach(schema => {
        html += `<div class="table-item">
            <h2>Table: ${schema.tablE_NAME}</h2>
            <table>
              <thead>
                <tr>
                  <th>Column Name</th>
                  <th>Data Type</th>
                </tr>
              </thead>
              <tbody>`;

        schema.columns.forEach(col => {
            html += `
                <tr>
                  <td>${col.columN_NAME}</td>
                  <td>${col.datA_TYPE}</td>
                </tr>`;
        });

        html += `</tbody></table></div>`;
    });

    html += `</div></body></html>`;
    return html;
}




