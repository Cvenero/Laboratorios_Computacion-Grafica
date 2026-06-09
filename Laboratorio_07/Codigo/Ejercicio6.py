import cv2

# Abrir la imagen en escala de grises
imagen = cv2.imread("Imagenes/Gato.jpg", cv2.IMREAD_GRAYSCALE)

# Aplicar umbral binario
# Parametros: imagen, valor umbral, valor maximo, tipo de umbral
# Si el pixel es mayor a 127 se convierte en 255 (blanco)
# Si el pixel es menor a 127 se convierte en 0 (negro)
_, threshold = cv2.threshold(imagen, 127, 255, cv2.THRESH_BINARY)

# Guardar la imagen
cv2.imwrite("Imagenes/threshold.jpg", threshold)
print("Imagen con umbral binario guardada correctamente")

# Mostrar ambas imagenes para comparar
cv2.imshow("Original en grises", imagen)
cv2.imshow("Umbral Binario", threshold)
cv2.waitKey(0)
cv2.destroyAllWindows()