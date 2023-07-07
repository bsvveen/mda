import { useState } from "react";

const useApi = (initialResponse) => {
    const [response, setResponse] = useState(initialResponse);
    const [error, setError] = useState(null);
    const [isLoading, setIsLoading] = useState(false);

    const parseJSON = (response) => {
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

    const apiFetch = async (url, payLoad) => {       
        setIsLoading(true);        
        await fetch(url, { 
                headers: new Headers({                   
                    "Accept" : "application/json",
                    "Content-Type" : "application/json",
                    "Cache" : "no-cache" }),                
                method: "POST",
                body: JSON.stringify(payLoad)})
            .then(parseJSON)
            .then((response) => {
                if (response.ok)
                    setResponse(response.json);
                if (!response.ok && response.status === 409)
                    setError(response.json); 
                if (!response.ok && response.status != 409)
                    throw new Error(JSON.stringify(response));  
                })           
            .catch(error => {alert(error)})
            .finally(setIsLoading(false));
    };     

    const fetchById = (entityName, id) => { return apiFetch('/User/GetById/', { "EntityName": entityName, "Id" : id}); }  

    const fetchList = (entityName, properties, constrains) => { return apiFetch("/User/List/", { "EntityName" : entityName, "Properties" : properties, "Constrains" : constrains}); }

    const update = (entityName, id, properties) => { return apiFetch('/User/Update/', { "EntityName" : entityName, "Id" : id, "Properties" : properties}); }

    return { response, error, isLoading, fetchById, fetchList, update };
}

export default useApi