using Microsoft.VisualStudio.TestTools.UnitTesting;
using SNFProtocol;
using System;
using System.IO;

namespace SNFProtocol.Tests
{
    [TestClass]
    public class PruebasSNF
    {
        [TestMethod]
        public void Codificacion_Decodificacion_ACK()
        {
            // Arrange
            SNF_ACK ack = new SNF_ACK { SequenceNumber = 5 };

            // Act
            byte[] encoded = ack.Encode();
            SNF_ACK decodedAck = new SNF_ACK();
            decodedAck.Decode(encoded);

            // Assert
            Assert.AreEqual(ack.SequenceNumber, decodedAck.SequenceNumber, "El número de secuencia en ACK no coincide.");
        }

        [TestMethod]
        public void Codificacion_Decodificacion_MESSAGE()
        {
            // Arrange
            SNF_DATA mensaje = new SNF_DATA { SequenceNumber = 10, Data = 200 };

            // Act
            byte[] encoded = mensaje.Encode();
            SNF_DATA decodedMessage = new SNF_DATA();
            decodedMessage.Decode(encoded);

            // Assert
            Assert.AreEqual(mensaje.SequenceNumber, decodedMessage.SequenceNumber, "El número de secuencia en MESSAGE no coincide.");
            Assert.AreEqual(mensaje.Data, decodedMessage.Data, "El contenido del mensaje no coincide.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Decodificacion_ERRONEA_ACK()
        {
            // Datos incorrectos (más de 1 byte)
            byte[] datosInvalidos = { 5, 10 };
            SNF_ACK ack = new SNF_ACK();
            ack.Decode(datosInvalidos);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Decodificacion_ERRONEA_MESSAGE()
        {
            // Datos incorrectos (menos de 2 bytes)
            byte[] datosInvalidos = { 10 };
            SNF_DATA mensaje = new SNF_DATA();
            mensaje.Decode(datosInvalidos);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Decodificacion_ERRONEA_MESSAGE_TamanoIncorrecto()
        {
            // Datos incorrectos (más de 2 bytes)
            byte[] datosInvalidos = { 10, 20, 30 };
            SNF_DATA mensaje = new SNF_DATA();
            mensaje.Decode(datosInvalidos);
        }
    }
}

