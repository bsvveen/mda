import React from 'react';
import { Routes, Route } from "react-router-dom";

import ListCustomers from "./pages/listcustomers";

function App() {    

    React.useEffect(() => {
        const fetchModel = async () => {
          fetch('/User/GetModel')
          .then((res) => res.json())
          .then(json => JSON.stringify(json))
          .then((data) => sessionStorage.setItem("model", data));                  
        };
        fetchModel();
    }, []);   

    return (
        <div className="App">
          <h1>Welcome to React Router!</h1>
          <Routes>
               <Route path="/" exact element={<ListCustomers />} />                
          </Routes>
        </div>
      );
}

export default App;