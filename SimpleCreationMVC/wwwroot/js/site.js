/*const { ajaxPrefilter } = require("jquery");*/

const getConnectionString = () => localStorage.getItem("connectionString") ?? encodeURI($(`[data-input="connectionString"]`).val());

const handleDapperQueryDownload = () => {
    window.open(`api/dapper-query/download-all?connectionString=${getConnectionString()}`)
}

const handleDapperProcedureDownload = () => {
    window.open(`api/dapper-stored-procedure/download-all?connectionString=${getConnectionString()}`)
}
   
const handleEFCoreDownload = () => {
    window.open(`api/ef-core/download-all?connectionString=${getConnectionString()}`)
}

const handleModelDownload = () => {
    window.open(`api/model/download-all?connectionString=${getConnectionString()}`)
}
   
const handleTsTypesDownload = () => {
    window.open(`api/front-end/ts-types-download?connectionString=${getConnectionString()}`)
}

const handleJsClassesDownload = () => {
    window.open(`api/front-end/js-classes-download?connectionString=${getConnectionString()}`)
}

const handleSystemPublishedDownload = () => {
    window.open(`api/system/download`)
}
   


let buttons = [
    {
        label: "Dapper Query Download",
        onClick: `handleDapperQueryDownload()`,

    },
    {
        label: "Dapper Stored Procedure Download",
        onClick: `handleDapperProcedureDownload()`,

    },
    {
        label: "EF Core Download",
        onClick: `handleEFCoreDownload()`,

    },
    {
        label: "Model Download",
        onClick: `handleModelDownload()`,

    },
    {
        label: "TypeScript-Types Download",
        onClick: `handleTsTypesDownload()`,

    },
    {
        label: "JavaScript-Classes Download",
        onClick: `handleJsClassesDownload()`,
    },
    {
        label: "System Published Download",
        onClick: `handleSystemPublishedDownload()`,
        btnColor: "success"
    }
];

$(_ => {
    generateButtons();
    connectionStringOnChange()
})

const connectionStringOnChange = () => {
    $(`[data-input="connectionString"]`).val(getConnectionString());
   
    $(`[data-input="connectionString"]`).on("keyup", (e) => {
        
        localStorage.setItem("connectionString", e.target.value);
        disabledButtons();
    })
}
const disabledButtons = () => {
    $(".download-btn").prop("disabled", !getConnectionString())
}

const generateButtons = () => {
    buttons.map(btn => {
        $("#botton-download-container").append(`
        <button type="button" class="download-btn btn btn-${btn.btnColor ?? 'primary'} col-lg-3" onClick=` + `${btn.onClick}` + `> ${btn.label} </button>
    `);
    })
    disabledButtons();
}


