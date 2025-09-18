import React, { useState, useEffect } from 'react';
import {
  View,
  Text,
  StyleSheet,
  Alert,
  Vibration,
  Dimensions,
  StatusBar,
  TouchableOpacity,
  PermissionsAndroid,
  Platform,
} from 'react-native';
import QRCodeScanner from 'react-native-qrcode-scanner';
import { NavigationProps, QRScanResult } from '@/types';
import MockDatabaseService from '@/services/MockDatabaseService';
import Icon from 'react-native-vector-icons/MaterialIcons';

const { width, height } = Dimensions.get('window');

interface QRScannerScreenProps extends NavigationProps {}

const QRScannerScreen: React.FC<QRScannerScreenProps> = ({ navigation }) => {
  const [isScanning, setIsScanning] = useState(true);
  const [flashOn, setFlashOn] = useState(false);
  const [scanning, setScanning] = useState(false);

  useEffect(() => {
    // El MockDatabaseService no necesita inicialización
    // Solicitar permisos de cámara en Android
    if (Platform.OS === 'android') {
      requestCameraPermission();
    }
  }, []);

  const requestCameraPermission = async () => {
    try {
      const granted = await PermissionsAndroid.request(
        PermissionsAndroid.PERMISSIONS.CAMERA,
        {
          title: 'Permiso de Cámara',
          message: 'Esta aplicación necesita acceso a la cámara para escanear códigos QR',
          buttonNeutral: 'Preguntar después',
          buttonNegative: 'Cancelar',
          buttonPositive: 'OK',
        }
      );
      
      if (granted !== PermissionsAndroid.RESULTS.GRANTED) {
        Alert.alert(
          'Permiso Denegado',
          'La aplicación necesita permisos de cámara para funcionar correctamente'
        );
      }
    } catch (err) {
      console.warn(err);
    }
  };

  const onSuccess = async (e: QRScanResult) => {
    if (scanning) return; // Evitar múltiples escaneos simultáneos

    setScanning(true);
    setIsScanning(false);
    
    // Vibración al escanear
    Vibration.vibrate(100);

    try {
      const qrData = e.data.trim();
      console.log('QR escaneado:', qrData);

      // Buscar la mercancía en la base de datos
      const response = await MockDatabaseService.getMercanciaByDocumento(qrData);

      if (response.success && response.data) {
        // Navegar a la pantalla de detalles con los datos encontrados
        navigation.navigate('MercanciaDetails', {
          mercancia: response.data,
          qrCode: qrData
        });
      } else {
        // Mostrar mensaje de error si no se encuentra la mercancía
        Alert.alert(
          'Mercancía no encontrada',
          `No se encontró ninguna mercancía con el código: ${qrData}`,
          [
            {
              text: 'Escanear otro',
              onPress: () => {
                setScanning(false);
                setIsScanning(true);
              }
            }
          ]
        );
      }
    } catch (error) {
      console.error('Error al procesar QR:', error);
      Alert.alert(
        'Error',
        'Ocurrió un error al procesar el código QR. Inténtalo nuevamente.',
        [
          {
            text: 'Reintentar',
            onPress: () => {
              setScanning(false);
              setIsScanning(true);
            }
          }
        ]
      );
    }

    // Resetear el estado de escaneo después de un tiempo
    setTimeout(() => {
      setScanning(false);
    }, 2000);
  };

  const toggleFlash = () => {
    setFlashOn(!flashOn);
  };

  const resetScanner = () => {
    setIsScanning(true);
    setScanning(false);
  };

  if (!isScanning) {
    return (
      <View style={styles.loadingContainer}>
        <Text style={styles.loadingText}>Procesando código QR...</Text>
        <TouchableOpacity style={styles.resetButton} onPress={resetScanner}>
          <Text style={styles.resetButtonText}>Escanear otro código</Text>
        </TouchableOpacity>
      </View>
    );
  }

  return (
    <View style={styles.container}>
      <StatusBar barStyle="light-content" backgroundColor="#000" />
      
      {/* Header */}
      <View style={styles.header}>
        <Text style={styles.title}>Escáner QR - Aduana</Text>
        <Text style={styles.subtitle}>Apunta la cámara hacia el código QR</Text>
      </View>

      {/* Scanner */}
      <QRCodeScanner
        onRead={onSuccess}
        flashMode={
          flashOn
            ? QRCodeScanner.Constants.FlashMode.torch
            : QRCodeScanner.Constants.FlashMode.off
        }
        topContent={null}
        bottomContent={
          <View style={styles.bottomContainer}>
            <TouchableOpacity style={styles.flashButton} onPress={toggleFlash}>
              <Icon 
                name={flashOn ? 'flash-off' : 'flash-on'} 
                size={30} 
                color="#fff" 
              />
              <Text style={styles.flashText}>
                {flashOn ? 'Apagar Flash' : 'Encender Flash'}
              </Text>
            </TouchableOpacity>
          </View>
        }
        cameraStyle={styles.camera}
        customMarker={
          <View style={styles.markerContainer}>
            <View style={styles.marker}>
              <View style={[styles.corner, styles.topLeft]} />
              <View style={[styles.corner, styles.topRight]} />
              <View style={[styles.corner, styles.bottomLeft]} />
              <View style={[styles.corner, styles.bottomRight]} />
            </View>
          </View>
        }
      />
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#000',
  },
  header: {
    padding: 20,
    backgroundColor: 'rgba(0, 0, 0, 0.8)',
    alignItems: 'center',
  },
  title: {
    fontSize: 22,
    fontWeight: 'bold',
    color: '#fff',
    marginBottom: 5,
  },
  subtitle: {
    fontSize: 16,
    color: '#ccc',
  },
  camera: {
    height: height * 0.7,
  },
  markerContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
  },
  marker: {
    width: 250,
    height: 250,
    position: 'relative',
  },
  corner: {
    position: 'absolute',
    width: 30,
    height: 30,
    borderColor: '#00ff00',
    borderWidth: 3,
  },
  topLeft: {
    top: 0,
    left: 0,
    borderRightWidth: 0,
    borderBottomWidth: 0,
  },
  topRight: {
    top: 0,
    right: 0,
    borderLeftWidth: 0,
    borderBottomWidth: 0,
  },
  bottomLeft: {
    bottom: 0,
    left: 0,
    borderRightWidth: 0,
    borderTopWidth: 0,
  },
  bottomRight: {
    bottom: 0,
    right: 0,
    borderLeftWidth: 0,
    borderTopWidth: 0,
  },
  bottomContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: 'rgba(0, 0, 0, 0.8)',
    paddingVertical: 30,
  },
  flashButton: {
    flexDirection: 'row',
    alignItems: 'center',
    backgroundColor: 'rgba(255, 255, 255, 0.2)',
    paddingHorizontal: 20,
    paddingVertical: 15,
    borderRadius: 25,
  },
  flashText: {
    color: '#fff',
    fontSize: 16,
    marginLeft: 10,
  },
  loadingContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: '#000',
  },
  loadingText: {
    fontSize: 18,
    color: '#fff',
    marginBottom: 30,
  },
  resetButton: {
    backgroundColor: '#007AFF',
    paddingHorizontal: 30,
    paddingVertical: 15,
    borderRadius: 25,
  },
  resetButtonText: {
    color: '#fff',
    fontSize: 16,
    fontWeight: 'bold',
  },
});

export default QRScannerScreen;