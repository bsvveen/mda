
const privateMethods = {

    Fetch(path, method, data) {
        return new Promise((resolve, reject) => {
            var url = path;
            if (data !== undefined && method === 'GET')
                url = url + '?' + data;

            return fetch(url, {
                headers: new Headers({                   
                    "Accept" : "application/json",
                    "Content-Type" : "application/json",
                    "Cache" : "no-cache"
                }),                
                method: method,
                body: JSON.stringify(data)
            }).then(privateMethods.parseJSON).then((response) => {
                if (response.ok)
                    return resolve(response.json); 
                if (response.status === 400)
                    alert(response.json);               
                if (response.status === 401)
                    alert("Toegang geweigerd: 401 (Unauthorized)");

                return reject(response.json);
            }).catch((err) => {
                alert("Kan de server niet bereiken, oorzaak: " + err);
            });
        })
    },

    parseJSON(response) {
        const contentType = response.headers.get("content-type");
            
        if (contentType && contentType.indexOf("application/json") !== -1) {
            return new Promise((resolve) => response.json()
                .then((json) => resolve({
                    status: response.status,
                    ok: response.ok,
                    json,
                })));
        }

        throw new Error("Geen informatie ontvangen van de server, " + response.status);
    }
}

export class Repository {

    static instance;

    constructor(entity) {
        if (this.instance) { return this.instance; }   
        this.entity = entity;
        this.instance = this;
    } 

    GetById = (Id) => {
        let data = { "Id" : Id, "Entity" : this.entity}
        return privateMethods.Fetch.call(this, '/User/GetById/', 'POST', data);
    }

    List = (properties, constrains) => {
        let data = { "Properties" : properties, "Entity" : this.entity, "Constrains" : constrains}
        return privateMethods.Fetch.call(this, '/User/List/', 'POST', data);
    } 
    
    Submit = (data) => {
        return privateMethods.Fetch.call(this, '/User/Submit/', 'POST', data);       
    } 

    Delete = (Id) => {
        return privateMethods.Fetch.call(this, "/" + this.controller + "/" + Id + '/Delete', 'DELETE');
    }

    ForeignKey = (endpoint, constrain, filter) => { 
        if (constrain) { 
            return privateMethods.Fetch.call(this, "/" + this.controller + "/" + endpoint + "/" + constrain, 'POST');
        } if (filter) { 
            let data = { "fields": [], "filter": filter}
            return privateMethods.Fetch.call(this, "/" + this.controller + "/" + endpoint, 'POST', data);
        } else {
            return privateMethods.Fetch.call(this, "/" + this.controller + "/" + endpoint, 'POST');
        }
    }
}

export default Repository;