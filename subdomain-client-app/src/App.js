import React, { useState } from 'react';
import { makeStyles } from '@material-ui/core/styles';
import { TextField, Button, Paper, TableContainer, Table, TableHead, TableRow, TableCell, TableBody, TablePagination, TableFooter } from '@material-ui/core';
import './App.css';
import axios from 'axios';


const useStyles = makeStyles((theme) => ({
  root: {
    width: '70%',
    margin: 'auto',
    textAlign: 'center',
    paddingTop: '50px'
  },
  formRoot: {
    '& > *': {
      margin: theme.spacing(1),
      width: '25ch',
    },
  },
  container: {
    maxHeight: 420,
  },
}));

function App() {

  const initialState = {
    DummyRows: { domain: { subdomains: [] } }
  };

  const [domain, setDomain] = useState('');
  const [DummyRows, setDummyRows] = useState(initialState.DummyRows);

  const classes = useStyles();
  const [page, setPage] = useState(0);
  const [rowsPerPage, setRowsPerPage] = useState(10);

  const handleChangePage = (event, newPage) => {
    setPage(newPage);
  };

  const handleChangeRowsPerPage = (event) => {
    setRowsPerPage(+event.target.value);
    setPage(0);
  };

  // Load subdomain from API
  const getSubdomains = async (domain, from) => {

    const apiUrl = "https://localhost:5001/subdomain/enumerate/" + domain;
    await axios.get(apiUrl, {
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json; charset=utf-8 ',
        'Access-Control-Allow-Origin': '*',
        "Access-Control-Allow-Methods": "*"
      }
    }).then(function (response) {

      let newDummyRows;
      if (from === "listSubdomain") {
        newDummyRows = Object.assign({}, initialState.DummyRows);
      } else if (from === "loadMore") {
        newDummyRows = Object.assign({}, DummyRows);
      }

      // eslint-disable-next-line array-callback-return
      response.data.map((subdomain) => {
        let rowCount = newDummyRows.domain.subdomains.length;
        newDummyRows.domain.subdomains.push({ key: rowCount + 1, name: subdomain, ips: [] })
      })

      setDummyRows(newDummyRows);
    })
      .catch(function (error) {
        // handle error
        console.log(error);
      })
      .then(function () {
        //Always Executed
      });

  }

  // Find IP Address for each host
  const findIpAddress = async (host, id) => {
    // const content = "\"www.google.com\"";
    const content = "\"" + host + "\"";
    const apiUrl = "https://localhost:5001/subdomain/findipaddresses";
    await axios.post(apiUrl, content, {
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json; charset=utf-8 ',
        'Access-Control-Allow-Origin': '*',
        "Access-Control-Allow-Methods": "*"
      }
    }).then(function (response) {
      let newDummyRows = Object.assign({}, DummyRows);
      let subdomains = newDummyRows.domain.subdomains;
      console.log(response.data);
      if (response.data.length > 0) {
        // eslint-disable-next-line array-callback-return
        response.data.map((ip, i) => {
          if (id + 1 === subdomains[id].key)
            (subdomains[id].ips.length === response.data.length) ? void 0 : subdomains[id].ips.push(ip);
        })
        setDummyRows(newDummyRows);
      } else if (response.data.length === 0) {
        (newDummyRows.domain.subdomains[id].ips.length === 0) ?
          newDummyRows.domain.subdomains[id].ips.push(response.data) :
          void 0;
      }
    })
      .catch(function (error) {
        // handle error
        console.log(error.message);
        let newDummyRows = Object.assign({}, DummyRows);

        (newDummyRows.domain.subdomains[id].ips.length === 0) ? newDummyRows.domain.subdomains[id].ips.push("IP not found") : void 0;
        setDummyRows(newDummyRows);

      })
      .then(function () {
        //Always Executed
      });
  }

  // List all subdomain handler
  const handleListSubdomain = () => {
    setDummyRows(initialState.DummyRows);
    getSubdomains(domain, "listSubdomain");
  }

  // Find IpAddress handler
  const handleFindIpAddress = () => {
    // eslint-disable-next-line array-callback-return
    DummyRows.domain.subdomains.map((sub, id) => {
      findIpAddress(sub.name, id);
    })
  }

  // Load more handler
  const handleLoadMore = () => {
    getSubdomains(domain, "loadMore");
  }

  return (
    <Paper className={classes.root}>

      <h1>Subdomain Generator</h1>

      <form className={classes.formRoot} noValidate autoComplete="off" style={{ margin: "30px" }}>
        <TextField id="standard-basic" label="domain e.g. yahoo.com" onChange={e => setDomain(e.target.value)} />
        <Button variant="contained" onClick={handleListSubdomain}>List Subdomains</Button>
        <Button variant="contained" onClick={handleFindIpAddress}>Find IP Addresses</Button>
      </form>
      <TableContainer className={classes.container}>
        <Table stickyHeader aria-label="sticky table">
          <TableHead>
            <TableRow>
              <TableCell>#</TableCell>
              <TableCell>Subdomain</TableCell>
              <TableCell>IP Addresses</TableCell>
            </TableRow>
          </TableHead>
          <TableBody style={{ maxHeight: "80vh" }}>
            {DummyRows.domain && DummyRows.domain.subdomains.length ?
              DummyRows.domain.subdomains.slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage).map((row) => (
                <TableRow key={row.name}>
                  <TableCell component="th" scope="row">{row.key}</TableCell>
                  <TableCell>{row.name}</TableCell>
                  <TableCell>
                    {row.ips.map((ip, id) => " [ " + ip + " ] ")}
                  </TableCell>
                </TableRow>
              )) : <TableRow><TableCell colSpan="3" align="center">No item found</TableCell></TableRow>}
          </TableBody>

          {DummyRows.domain.subdomains.length > 0 ?
            <TableFooter>
              <TableRow>
                <TableCell colSpan="3" style={{ textAlign: 'center' }}>
                  <Button onClick={handleLoadMore}>
                    Load more
                  </Button>
                </TableCell>
              </TableRow>
            </TableFooter>
            : <TableFooter></TableFooter>}
        </Table>
      </TableContainer>
      <TablePagination
        rowsPerPageOptions={[10, 20, 30]}
        component="div"
        count={DummyRows.domain.subdomains.length}
        rowsPerPage={rowsPerPage}
        page={page}
        onChangePage={handleChangePage}
        onChangeRowsPerPage={handleChangeRowsPerPage}
      />
    </Paper>
  );
}

export default App;
