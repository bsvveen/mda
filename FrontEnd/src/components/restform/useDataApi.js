import React from 'react';

const dataFetchReducer = (state, action) => {
    switch (action.type) {     
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

const useDataApi = (initialData) => {  
  const [response, dispatch] = React.useReducer(dataFetchReducer, {
    isLoading: false,
    error: null,
    modelstate: null,
    data: initialData,
  });

  const setRequest = async (request) => {
    dispatch({ type: 'FETCH_INIT' });
    
      await apiFetch(request.url, request.payload)
      .then((res) => {
          if (res.status == "409")
            dispatch({ type: 'VALIDATION_FAILURE', payload: JSON.stringify(res.data) });
    
          dispatch({ type: 'FETCH_SUCCESS', payload: res.data });
      }) 
      .catch((error) => dispatch({ type: 'FETCH_FAILURE', payload: error.message + error.stack }))  
  };

  console.info("useDataApi", response);

  return [response, setRequest];
};

const useFetchList = () => {  
  const [response, setRequest] = useDataApi([]) 

  const setListRequest = (entityName, properties, constrains) => {
    setRequest({url: '/User/List/', payload: { "EntityName" : entityName, "Properties" : properties, "Constrains" : constrains,}})
  }

  return [response, setListRequest]
}

const useFetchById = (entityName, id) => {
  return useDataApi({}, {url: '/User/GetById/', payload: { "EntityName": entityName, "Id" : id }});     
}  

const useUpdate = (initialValue, initialRequest) => {
  const [response, setRequest] = useDataApi(initialValue, initialRequest) 

  const setUpdateRequest = (entityName, id, properties) => {
    setRequest({url: '/User/Update/', payload: { "EntityName" : entityName, "Id" : id, "Properties" : properties }})
  }

  return [response, setUpdateRequest]
}

export {useFetchById, useFetchList, useUpdate};