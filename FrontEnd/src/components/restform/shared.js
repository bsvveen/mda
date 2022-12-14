
function toDate(string) {
    if (string === undefined || string === "")
        return undefined;

    return new Date(string).toLocaleDateString('nl-NL', { year: 'numeric', month: 'long', day: 'numeric' });
}

function toDateTime(string) {
    if (string === undefined || string === "")
        return '...';

    return new Date(string).toLocaleDateString('nl-NL', { year: 'numeric', month: 'long', day: 'numeric', hour: 'numeric', minute: 'numeric' });
}

function getUniqueId() {
    return 'id-' + Math.random().toString(36).substr(2, 16);
};

function getModelInstance(model) {
    let ModelInstance = {};
    model.map((m) => { return ModelInstance[m.key] = "" });
    return ModelInstance;
};

function getQueryParameter(key) {
    var results = new RegExp('[\?&]' + key + '=([^&#]*)').exec(window.location.href);
    if (results==null)
       throw Error('Missing querystring parameter');
   
    else{
       return decodeURI(results[1]) || 0;
    }
}  

export default { toDate, toDateTime, getUniqueId, getQueryParameter, getModelInstance }