import React from 'react';
import { BrowserRouter, Routes, Route} from "react-router-dom";
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
      <div id="app">
        <BrowserRouter> 
          <div id="wrapper">   
              <Routes>
                <Route path="/" exact element={<ListCustomers />} />
              </Routes>                                   
          </div>             
        </BrowserRouter>
      </div>        
    );
}
 
export default App;