import React from 'react';
import Dashboard from './components/dashboard';

import importedViews from './views.json';
import ListAndNew from './components/restform/listandnew';

const componentFactory = {
	listAndNew: ListAndNew	
};

const gridProperties = { 
    breakpoints:{ lg: 1200, md: 996, sm: 768, xs: 480, xxs: 0 },
    cols:{ lg: 5, md: 4, sm: 3, xs: 2, xxs: 1 },
    rowHeight:300, width:1000
}

function App() {    
    const [views, setViews] = React.useState([]);

    React.useEffect(() => {
      const fetchModel = async () => {
        fetch('/User/GetModel')
        .then((res) => res.json())
        .then(json => JSON.stringify(json))
        .then((data) => sessionStorage.setItem("model", data));                  
      };

      fetchModel();
      setViews(importedViews.views);
    }, []);   

    if (views.length == 0) { return <div>Loading ...</div> } 
  
    return (<Dashboard views={views} componentFactory={componentFactory} gridProperties={gridProperties} />);
}
 
export default App;