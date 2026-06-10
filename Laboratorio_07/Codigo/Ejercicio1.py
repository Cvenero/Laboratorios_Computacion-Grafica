import cv2

# Abrir las tres imagenes
img1 = cv2.imread("Imagenes/Aguila.jpg")
img2 = cv2.imread("Imagenes/Gato.jpg")
img3 = cv2.imread("Imagenes/Lobo.jpg")

imagenes = [img1, img2, img3]

# Detectar automaticamente la imagen mas grande por numero de pixeles
mas_grande = max(imagenes, key=lambda img: img.shape[0] * img.shape[1])

ancho = mas_grande.shape[1]
alto = mas_grande.shape[0]

print("Dimensiones originales:")
print("Aguila:", img1.shape)
print("Gato:", img2.shape)
print("Lobo:", img3.shape)
print("")

print("Tamaño de la imagen mas grande:", alto, "x", ancho)

# Redimensionar las tres imagenes al tamaño de la mas grande
img1_redim = cv2.resize(img1, (ancho, alto))
img2_redim = cv2.resize(img2, (ancho, alto))
img3_redim = cv2.resize(img3, (ancho, alto))

print("Aguila:", img1_redim.shape)
print("Gato:", img2_redim.shape)
print("Lobo:", img3_redim.shape)

# Guardar las imagenes redimensionadas
cv2.imwrite("Imagenes/Aguila_redimensionada.jpg", img1_redim)
cv2.imwrite("Imagenes/Gato_redimensionado.jpg", img2_redim)
cv2.imwrite("Imagenes/Lobo_redimensionado.jpg", img3_redim)
print("")
print("Imagenes guardadas exitosamente")

# Mostrar las imagenes redimensionadas
cv2.imshow("Aguila", img1_redim)
cv2.imshow("Gato", img2_redim)
cv2.imshow("Lobo", img3_redim)

cv2.waitKey(0)  # Espera cualquier tecla para cerrar
cv2.destroyAllWindows()
