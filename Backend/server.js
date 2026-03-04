const express = require('express');
const mysql = require('mysql2');

const app = express();

const port = 3000;
const host = 'localhost';

app.use(express.json());
app.use(express.urlencoded({ extended: true }));

// szerver indítása
app.listen(port, host, () => {
    console.log(`A szerver figyel a http://${host}:${port} porton!`);
});

// teszt
app.get('/', (req, res) => {
    res.send('A szerver fut!');
});

// adatbázis kapcsolat
const connection = mysql.createConnection({
    host: 'localhost',
    user: 'root',
    password: '',
    database: 'penzugyek'
});

// adatbázis teszt
app.get('/test', (req, res) => {
    connection.query('SELECT 1', (err) => {

        if (err) {
            console.error('Adatbázis hiba!', err);
            return res.status(500).send('Nem sikerült kapcsolódni az adatbázishoz!');
        }

        res.send('Az adatbázis kapcsolat működik!');
    });
});

// BEVÉTEL HOZZÁADÁSA
app.post("/bevetel",(req,res)=>{
    const {forras, osszeg} = req.body;
    if (!forras||!osszeg) {
        return res.status(400).json({error:"Minden mezőt tölts ki!"});       
    }
    connection.query(
        'INSERT INTO bevetel (Forras, Osszeg) VALUES (?, ?)',
        [forras, osszeg],
        (err, result) => {

            if (err) {
                console.error(err);
                return res.status(500).json({ error: 'Adatbázis hiba!' });
            }

            return res.status(201).json({
                message: 'Bevétel rögzítve!',
            });
        }
    );
});

// KIADÁS HOZZÁADÁSA
app.post("/kiadas",(req,res)=>{
    const {kategoria, leiras, osszeg, datum} = req.body;
    if (!kategoria||!osszeg||!datum) {
        return res.status(400).json({error:"Minden mezőt tölts ki!"});       
    }
    connection.query(
        'INSERT INTO kiadas (Kategoria, Leiras, Osszeg, Datum) VALUES (?, ?, ?, ?)',
        [kategoria, leiras, osszeg, datum],
        (err, result) => {

            if (err) {
                console.error(err);
                return res.status(500).json({ error: 'Adatbázis hiba!' });
            }

            return res.status(201).json({
                message: 'Kiadás rögzítve!',
            });
        }
    );
});

// KIADÁSOK LISTÁJA
app.get('/kiadasok', (req, res) => {

    connection.query(
        'SELECT * FROM kiadas ORDER BY Datum DESC',
        (err, result) => {

            if (err) {
                return res.status(500).json({ error: 'Hiba a lekérdezés során!' });
            }

            res.json(result);
        }
    );
});

//teljes bevétel
app.get('/osszbevetel', (req, res) => {

    connection.query(
        'SELECT SUM(Osszeg) AS osszBevetel FROM bevetel',
        (err, result) => {

            if (err) {
                return res.status(500).json({ error: 'Hiba a bevétel lekérdezésnél!' });
            }

            const be = result[0].osszBevetel || 0;

            res.json({
                oszzBevetel: be
            });
        }
    );
});

//teljes kiadás
app.get('/osszkiadas', (req, res) => {

    connection.query(
        'SELECT SUM(Osszeg) AS osszKiadas FROM kiadas',
        (err, result) => {

            if (err) {
                return res.status(500).json({ error: 'Hiba a kiadás lekérdezésnél!' });
            }

            const ki = result[0].osszKiadas || 0;

            res.json({
                osszKiadas: ki
            });
        }
    );
});