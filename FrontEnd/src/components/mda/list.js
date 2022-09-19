import React from 'react';
import { Routes, Route } from "react-router-dom";

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
                <Route path="/" exact render={props => <TrajectBeheren {...model} />} />
                <Route path="/beheergebruikers" render={props => <BeheerGebruikers {...model} />} />  
          </Routes>
        </div>
      );
}

export default App;