import React from 'react';
import { Routes, Route } from "react-router-dom";

import ListCustomers from "./pages/listcustomers";

function App() {
    const [model, setModel] = React.useState({});

    React.useEffect(() => {
        const fetchData = async () => {
            const res = await fetch('/User/GetModelSchema');
            const json = await res.json();
            setModel(json);
        };
        fetchData();
    }, []);   

    return (
        <div className="App">
          <h1>Welcome to React Router!</h1>
          <Routes>
               <Route path="/" exact element={<ListCustomers model={model} />} />                
          </Routes>
        </div>
      );
}

export default App;