import React from 'react';

const dataFetchReducer = (state, action) => {
    switch (action.type) {
        case 'NO_FETCH':
            return {
                ...state,
                isLoading: false
            };
        case 'FETCH_INIT':
            return {
                ...state,
                isLoading: true
            };
        case 'FETCH_SUCCESS':
            return {
                ...state,
                isLoading: false,               
                data: action.payload
            };
        case 'VALIDATION_FAILURE':
            return {
                ...state,
                isLoading: false,
                modelstate: action.payload
            };
        case 'FETCH_FAILURE':
            return {
                ...state,
                isLoading: false,                
                error: action.payload
            };
        default:
            throw new Error();
    }
};

const parseJSON = (response) => {
  const contentType = response.headers.get("content-type");
      
  if (contentType && contentType.indexOf("application/json") !== -1) {
      return new Promise((resolve, reject) => response.json()
          .then((json) => resolve({
              status: response.status,
              ok: response.ok,
              data: json
          }))
          .catch((error) => reject("parseJSON error", error)));
  }  
}

const apiFetch = async (url, payLoad) => {  
  return new Promise((resolve, reject) => {
    fetch(url, { 
      headers: new Headers({                   
        "Accept" : "application/json",
        "Content-Type" : "application/json",
        "Cache" : "no-cache" }),                
        method: "POST",
        body: JSON.stringify(payLoad)})
      .then(parseJSON)
      .then((response) => { resolve(response); })
      .catch((error) => reject(error))
  })
}

const useDataApi = (initialData, initialRequest) => {    
    const [request, setRequest] = React.useState(initialRequest??{});

    const [response, dispatch] = React.useReducer(dataFetchReducer, {
      isLoading: false,
      error: null,
      modelstate: null,
      data: initialData,
    });
  
    React.useEffect(() => {
      const fetchData = async () => {
        dispatch({ type: 'FETCH_INIT' });
  
        await apiFetch(request.url, request.payload)
        .then((response) => {
            if (response.status == "409")
              dispatch({ type: 'VALIDATION_FAILURE', payload: JSON.stringify(response.data) });

            dispatch({ type: 'FETCH_SUCCESS', payload: response.data });
        }) 
        .catch((error) => dispatch({ type: 'FETCH_FAILURE', payload: error.message + error.stack }))  
      }     

      if (request == {})
        dispatch({ type: 'NO_FETCH', payload: response.data });

      fetchData();
    }, [request]);

    return [response, setRequest];
  };

const useFetchList = (entityName, properties, constrains) => {
    const listRequest = {url: '/User/List/', payload: { "EntityName" : entityName, "Properties" : properties, "Constrains" : constrains}}
    return useDataApi([], listRequest);
}

  const useFetchById = (entityName, id) => {
    return useDataApi({}, {url: '/User/GetById/', payload: { "EntityName": entityName, "Id" : id }});     
  }  

  const useUpdate2 = (initialValue, initialRequest) => {
    const [response, setRequest] = useDataApi(initialValue, initialRequest) 
  
    const setListRequest = (entityName, properties, constrains) => {
      setRequest({url: '/User/List/', payload: { "EntityName" : entityName, "Properties" : properties, "Constrains" : constrains}})
    }
  
    return [response, setListRequest]
}

  const useUpdate = () => {
    
    const doUpdate =  useDataApi([], {url: '/User/Update/', payload: { "EntityName" : entityName, "Id" : id, "Properties" : properties }}); 

    React.useEffect(() => {
      const fetchData = async () => {
        dispatch({ type: 'FETCH_INIT' });
  
        await apiFetch(request.url, request.payload)
        .then((response) => {
            if (response.status == "409")
              dispatch({ type: 'VALIDATION_FAILURE', payload: JSON.stringify(response.data) });

            dispatch({ type: 'FETCH_SUCCESS', payload: response.data });
        }) 
        .catch((error) => dispatch({ type: 'FETCH_FAILURE', payload: error.message + error.stack }))  
      }     

      fetchData();
    }, [request]);
    
    return [updateResponse, setRequest];
  }

  export {useFetchById, useFetchList, useUpdate};