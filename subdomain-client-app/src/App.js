import React, { useState } from 'react';
import { makeStyles } from '@material-ui/core/styles';
import { TextField, Button, Container, Paper, TableContainer, Table, TableHead, TableRow, TableCell } from '@material-ui/core';
import './App.css';


const useStyles = makeStyles((theme) => ({
  root: {
    '& > *': {
      margin: theme.spacing(1),
      width: '25ch',
    },
  },
  table: {
    minWidth: 650,
  },
}));

function App() {
  const [subdomains, setSubdomains] = useState([]);

  const getSubdomains = (domain) => {
    console.log(domain);
  }

  const handleListSubdomain = (e) => {
    const domain = document.getElementById('domainTextField').value;

    getSubdomains(domain);
  }

  const classes = useStyles();
  return (
    <Container>
      <Paper elevation={3} />
      <form className={classes.root} noValidate autoComplete="off">
        <TextField id="domainTextField" label="domain e.g. yahoo.com" />
        <Button variant="contained" onClick={handleListSubdomain}>List Subdomains</Button>
        <Button variant="contained">Find IP Addresses</Button>
      </form>

      <TableContainer component={Paper}>
        <Table className={classes.table} aria-label="simple table">
          <TableHead>
            <TableRow>
              <TableCell>#</TableCell>
              <TableCell align="right">Subdomain</TableCell>
              <TableCell align="right">IP Addresses</TableCell>
            </TableRow>
          </TableHead>
        </Table>
      </TableContainer>
    </Container>

  );
}

export default App;
